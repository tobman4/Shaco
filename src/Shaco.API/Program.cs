global using Shaco.API.Data.Models;

using Shaco.API.Data;
using Shaco.API.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

builder.Services.AddScoped<TokenService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddDbContext<DB, SqliteDB>();

var app = builder.Build();

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
