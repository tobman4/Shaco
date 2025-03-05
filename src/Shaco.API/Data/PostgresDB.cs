using Microsoft.EntityFrameworkCore;

namespace Shaco.API.Data;

public class PostgresDB(
  IConfiguration config
) : DB {

  protected override void OnConfiguring(DbContextOptionsBuilder builder) {
    builder.UseNpgsql(config["Postgres"]!);
    
  }

}
