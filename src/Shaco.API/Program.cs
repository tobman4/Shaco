global using Shaco.API.Data.Models;
using Microsoft.EntityFrameworkCore;
using Shaco.API.Data;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

builder.Services.AddDbContext<DB, SqliteDB>();

var app = builder.Build();

app.UseHttpsRedirection();
app.MapControllers();

using(var scope = app.Services.CreateAsyncScope()) {
  var db = scope.ServiceProvider.GetRequiredService<DB>();
  var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

  logger.LogDebug("DB: {con}", db.Database.GetConnectionString());
  if(db.Database.GetPendingMigrations().Count() > 0) {
    logger.LogInformation("Updating db");
    await db.Database.MigrateAsync();
  }

}

app.Run();
