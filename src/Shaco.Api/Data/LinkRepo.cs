using Shaco.Contract.DTOs;

namespace Shaco.Api.Data;

public class MvpLinkRepo : ILinkRepo {
  
  private static List<LinkDTO> _links = new();


  public LinkDTO Add(LinkDTO data) {
    _links.Add(data);
    return data;
  }

  public LinkDTO? ByID(Guid id) => _links.SingleOrDefault(e => e.ID == id);
  public LinkDTO? ByName(string name) => _links.SingleOrDefault(e => e.Name == name);

  public void Delete(Guid id) {
    if(ByID(id) is LinkDTO link) _links.Remove(link);
  }

  public IEnumerable<LinkDTO> GetAll() => _links.ToArray();

  public void Save() {
  }

  public Task SaveAsync() {
    return Task.CompletedTask;
  }
}
