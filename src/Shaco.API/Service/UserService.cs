using System.Security.Cryptography;
using System.Text;
using Shaco.API.Data;

namespace Shaco.API.Services;

public class UserService(
  DB db,
  ILogger<UserService> logger
) {
  
  private readonly Random _rng = new();
  private readonly ILogger _logger = logger;
  private readonly DB _db = db;

  private IEnumerable<byte> GetSalt(int length=256) {
    byte[] salt = new byte[length];
    _rng.NextBytes(salt);

    return salt;
  }

  private string Hash(string raw, byte[] salt) {
    var rawBytes = Encoding.UTF8.GetBytes(raw);
    var salted = new byte[rawBytes.Length + salt.Count()];

    rawBytes.CopyTo(salted, 0);
    salt.CopyTo(salted,rawBytes.Length);
    

    var hash = SHA256.Create().ComputeHash(salted);

    return Convert.ToHexString(hash);
  }

  public User AddUser(string name, string password, string role = "User") {
    var saltBytes = GetSalt().ToArray();
    var hash = Hash(password, saltBytes);
    
    _logger.LogInformation("Adding new user {user}", name);
    var user = _db.Users.Add(new User {
      Username = name,
      Hash = hash,
      Salt = Convert.ToHexString(saltBytes),
      Role = role
    }).Entity;

    _db.SaveChanges();
    return user;
  }

  public User? TryAuth(string username, string password) {
    var user = _db.Users.SingleOrDefault(e => e.Username == username);
    if(user is null)
      return null;

    var salt = Convert.FromHexString(user.Salt);
    var hash = Hash(password,salt);

    if(user.Hash != hash)
      return null;

    return user;
  }

  public IEnumerable<User> GetAll() =>
    _db.Users.ToArray();

  public User? tryGetByID(Guid id) =>
    _db.Users.SingleOrDefault(e => e.ID == id);

  public void DeleteUser(User user) {
    _db.Users.Remove(user);
    _db.SaveChanges();
  }
}
