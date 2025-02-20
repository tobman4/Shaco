namespace Shaco.API.Data.Models;

public class User {
  public Guid ID { get; init; } = Guid.NewGuid();
  public string Username { get; set; } = null!;
  public string Role { get; set; } = "User";

  public string Hash { get; set; } = null!;
  public string Salt { get; set; } = null!;
}
