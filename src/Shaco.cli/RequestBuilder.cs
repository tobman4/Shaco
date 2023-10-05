using System.Net.Http.Json;
using System.Text;

namespace Shaco.cli;

public class RequestBuilder {

  private Dictionary<string,string> _header = new();
  private object? _body = null;
  private string _path = "";
  private HttpMethod _method = HttpMethod.Get;

  public RequestBuilder IsDelete() => UseMethod(HttpMethod.Delete);
  public RequestBuilder IsPost() => UseMethod(HttpMethod.Post);
  public RequestBuilder UseMethod(HttpMethod method) {
    _method = method;
    return this;
  }

  public RequestBuilder SetBody(object body) {
    _body = body;
    return this;
  }

  public RequestBuilder AddHeader(string key,string val) {
    _header.Add(key,val);
    return this;
  }

  public RequestBuilder UsePath(string path) {
    _path = path;
    return this;
  }

  public RequestBuilder BasicAuth(string username, string password) {
    var bytes = Encoding.UTF8.GetBytes($"{username}:{password}");
    AddHeader("Authorization", $"Basic {Convert.ToBase64String(bytes)}");

    return this;
  }

  public HttpRequestMessage Build() {
  
    var req = new HttpRequestMessage(_method,_path);

    if(_body is not null) {
      req.Content = JsonContent.Create(_body);
    }

    foreach(var kv in _header) req.Headers.Add(kv.Key,kv.Value);

    return req;
  }
}
