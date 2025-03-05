global using Shaco.API.Data.Models;

using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Shaco.API.Data;
using Shaco.API.Services;

var builder = WebApplication.CreateBuilder(args);
var password = builder.Configuration["Secret"]!;

builder.Services.AddControllers();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
  .AddJwtBearer(e => {
    e.TokenValidationParameters = new TokenValidationParameters {
    ValidateIssuer = true,
    ValidateAudience = true,
    ValidateLifetime = true,
    ValidateIssuerSigningKey = true,
    
    RoleClaimType = "Role",
    ValidIssuer = "Shaco.API",
    ValidAudience = "Shaco.API",
    IssuerSigningKey = new SymmetricSecurityKey(
      Encoding.UTF8.GetBytes(password)
    )

    };
  });

builder.Services.AddAuthorization();

builder.Services.AddScoped<TokenService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddDB(builder.Configuration);

var app = builder.Build();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.SetupDB();

app.Run();
