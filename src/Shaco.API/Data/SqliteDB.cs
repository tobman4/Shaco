using Microsoft.EntityFrameworkCore;

namespace Shaco.API.Data;

public class SqliteDB : DB {
  
  private readonly string DbPath;

  public SqliteDB(
    IConfiguration config
  ) {
    var path = config["Sqlite"];
    if(string.IsNullOrWhiteSpace(path))
      throw new Exception("No db path");

    DbPath = path;
  }

  /* public SqliteDB() { */
  /*   var folder = Environment.SpecialFolder.LocalApplicationData; */
  /*   var path = Environment.GetFolderPath(folder); */
  /*   DbPath = System.IO.Path.Join(path, "shaco.db"); */
  /* } */

  protected override void OnConfiguring(DbContextOptionsBuilder options) =>
    options.UseSqlite($"Data Source={DbPath}");
}
