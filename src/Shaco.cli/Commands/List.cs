

using System.CommandLine;
using System.Net.Http.Json;
using System.Text.Json;
using Shaco.Contract.DTOs;
using Spectre.Console;

namespace Shaco.cli.Commands;


public static class Link {

  private static HttpClient? _c;
  private static HttpClient _client {
    get {
      if(_c is null) _c = App.Instance.GetClient(true);
      return _c;
    }
  }

  public static readonly Option<string> NameOpt = new(new string[] { "--name","-n" }, "Name for the link. Can be used as key");
  public static readonly Option<bool> FormatOpt = new(new string[] { "--use-uri","-f" }, "Use the riderect uri insted of name");

  public static readonly Argument<string> LinkArg = new("Link", "Id or name of a link");
  public static readonly Argument<Uri> DestArg = new("dest");

  private static async Task<T?> TrySendAsync<T>(HttpRequestMessage req) where T : class {
    var res = await _client.SendAsync(req);

    if((int)res.StatusCode == 404) return null;
    res.EnsureSuccessStatusCode();

    var obj = await res.Content.ReadFromJsonAsync<T>();
    if(obj is null) throw new JsonException("Got bad json");
    
    return obj;
  }

  private static async Task<T> SendAsync<T>(HttpRequestMessage req) where T : class {
    var res = await _client.SendAsync(req);
    res.EnsureSuccessStatusCode();

    var obj = await res.Content.ReadFromJsonAsync<T>();
    if(obj is null) throw new JsonException("Got bad json");

    return obj;
  }

  private static async Task<LinkDTO?> GetLinkAsync(string key) {
    var req = new RequestBuilder()
      .UsePath($"api/Link/{key}")
      .Build();

    return await TrySendAsync<LinkDTO>(req);
  }

  public static async Task List(bool format) {
    var req = new RequestBuilder()
      .UsePath("api/Link")
      .Build();

    var links = await SendAsync<IEnumerable<LinkDTO>>(req);
    var server = App.Instance.GetServer();

    AnsiConsole.MarkupLine($"Found {links.Count()} links");
    foreach(var link in links) {
      string key;
      if(format) key = $"{new Uri(server,$"api/Link/f/{link.Name}")}";
      else key = link.Name;

      AnsiConsole.WriteLine($"{key} => {link.Uri.AbsoluteUri}");
    }
  }

  public static async Task Delete(string key) {
    var link = await GetLinkAsync(key);
    if(link is null) {
      AnsiConsole.WriteLine($"Found no key {key}");
      return;
    }

    var req = new RequestBuilder()
      .UsePath($"api/Link/{link.ID}")
      .IsDelete()
      .Build();

    var res = await _client.SendAsync(req);
    res.EnsureSuccessStatusCode();

    AnsiConsole.MarkupLine("Delete [green]ok[/]");
  }

  public static async Task Add(Uri dst, string name) {
    if(!dst.IsAbsoluteUri) {
      AnsiConsole.MarkupLine("Bad uri");
      return;
    }

    Console.WriteLine($"dst: {dst.AbsoluteUri}");    
    
    var body = new NewLinkDTO {
      Name = name,
      Dest = dst
    };

    var req = new RequestBuilder()
      .IsPost()
      .UsePath("api/Link")
      .SetBody(body)
      .Build();

    var res = await _client.SendAsync(req); 
    res.EnsureSuccessStatusCode();

    var json = await res.Content.ReadFromJsonAsync<LinkDTO>();
    if(json is null) {
      AnsiConsole.MarkupLine("Got bad json");
      return;
    }

    AnsiConsole.MarkupLine($"Link id: {json.ID}");
  }

  public static void AddCommands(Command command) {
    var add = new Command("add");
    add.AddArgument(DestArg);
    add.AddOption(NameOpt);
    add.SetHandler(Add,DestArg,NameOpt);
    command.Add(add);

    var list = new Command("list");
    list.SetHandler(List,FormatOpt);
    list.Add(FormatOpt);
    command.Add(list);

    var delete = new Command("del");
    delete.Add(LinkArg);
    delete.SetHandler(Delete,LinkArg);
    command.Add(delete);
  }
}
