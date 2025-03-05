using Microsoft.EntityFrameworkCore;

namespace Shaco.API.Data;

static class DbExtensions {

  public static void AddDB(this IServiceCollection services, IConfiguration config) {
    if(!string.IsNullOrWhiteSpace(config["Sqlite"]))
      services.AddDbContext<DB,SqliteDB>();

    else if(!string.IsNullOrWhiteSpace(config["Postgres"]))
      services.AddDbContext<DB,PostgresDB>();

    else
      throw new Exception("No db config");
  }

  public static void SetupDB(this WebApplication app) {
    var services = app.Services.CreateAsyncScope().ServiceProvider;

    var logger = services.GetRequiredService<ILogger<DB>>();
    var db = services.GetRequiredService<DB>();

    logger.LogInformation("Using {provider}", db.GetType().Name);
    
    if(db.Database.GetPendingMigrations().Count() > 0) {
      logger.LogWarning("Updating db");
      db.Database.Migrate();
    }
  }

}
