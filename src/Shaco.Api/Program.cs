global using Shaco.Contract.Interfaces;

using Shaco.Api.Authentication;
using Shaco.Api.Data;
using Fluffy.AspNet;
using Fluffy;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DB>();

builder.Services.AddScoped<ILinkRepo,LinkRepo>();

builder.Services.AddFluffyAuth();

/* builder.Services.AddAuthentication(auth => { */
/*   auth.AddScheme<BasicAuthentication>("basic","basic"); */

/*   auth.DefaultScheme = "basic"; */
/* }); */

builder.Services.AddAuthorization(auth => {

  auth.AddPolicy("Admin", e => {
    e.RequireClaim("Role","Admin","Owner");

    e.AddAuthenticationSchemes("Token");
  });
  
  
});

builder.Services.AddControllers();

var app = builder.Build();

using(var s = app.Services.CreateAsyncScope()) {
  var a = s.ServiceProvider.GetRequiredService<IAuthClient>();
}

app.MapGet("/", (context) => {
  foreach(var c in context.User.Claims) Console.WriteLine($"{c.Type}: {c.Value}");

  return Task.FromResult("Hello");
});

app.UseHttpLogging();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

using(var scope = app.Services.CreateAsyncScope()) {
  var db = scope.ServiceProvider.GetRequiredService<DB>();
  await db.Database.EnsureCreatedAsync();
}

app.Run();
