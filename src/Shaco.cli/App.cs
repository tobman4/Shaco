using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Spectre.Console;

namespace Shaco.cli;

internal class App {
  
  public static App Instance = null!;

  private static IHost _app = null!;
  private readonly IServiceScope _commandScope;

  public App() {
    if(Instance is not null) throw new Exception("no");
    Instance = this;

    var builder = Host.CreateDefaultBuilder();

    builder.ConfigureServices(ConfigService);

    builder.ConfigureLogging((context,builder) => {

      if(context.HostingEnvironment.IsProduction()) {
        builder.ClearProviders();
      }

    });

    _app = builder.Build();
    _commandScope = _app.Services.CreateAsyncScope();
  }

  private void ConfigService(HostBuilderContext context, IServiceCollection services) {
    var handler = new HttpClientHandler {
      ServerCertificateCustomValidationCallback = (request, cert, chain, errors) => true
    };

    services.AddHttpClient("shaco", e => {
      e.BaseAddress = new Uri("https://shaco.tobman.no");
    }).ConfigurePrimaryHttpMessageHandler(() => handler);
  }

  public Uri GetServer() {
    return new Uri("https://shaco.tobman.no");
  }

  public HttpClient GetClient(bool useAuth = true) {
    var factory = _commandScope.ServiceProvider.GetRequiredService<IHttpClientFactory>();
    var client = factory.CreateClient("shaco");

    if(useAuth) {
      string username = AnsiConsole.Prompt(
        new TextPrompt<string>("Username: ")
          .DefaultValue("Shaco")
      );

      string password = AnsiConsole.Prompt(
        new TextPrompt<string>("Password: ")
          .DefaultValue("Shaco")
          .Secret()
        );

      var bytes = Encoding.UTF8.GetBytes($"{username}:{password}");
      var auth = Convert.ToBase64String(bytes);

      client.DefaultRequestHeaders.Authorization = new("Basic", auth);
    }

    return client;
  }
}
