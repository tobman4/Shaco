global using Shaco.Contract.Interfaces;

using Shaco.Api.Authentication;
using Shaco.Api.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DB>();

builder.Services.AddScoped<ILinkRepo,LinkRepo>();

builder.Services.AddAuthentication(auth => {
  auth.AddScheme<BasicAuthentication>("basic","basic");

  auth.DefaultScheme = "basic";
});

builder.Services.AddAuthorization(auth => {

  auth.AddPolicy("Admin", e => {
    e.RequireRole("admin");
  });
  
  
});

builder.Services.AddControllers();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.UseHttpLogging();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

using(var scope = app.Services.CreateAsyncScope()) {
  var db = scope.ServiceProvider.GetRequiredService<DB>();
  await db.Database.EnsureCreatedAsync();
}

app.Run();
