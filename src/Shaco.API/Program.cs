global using Shaco.API.Data.Models;

using Shaco.API.Data;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

builder.Services.AddDbContext<DB, SqliteDB>();

var app = builder.Build();

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
