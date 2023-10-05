using Shaco.Contract.DTOs;

namespace Shaco.Api.Data;

public class LinkRepo : ILinkRepo {
  private readonly DB _db;

  public LinkRepo(DB db) {
    _db = db;

  }

  public LinkDTO Add(LinkDTO data) => _db.Links.Add(data).Entity;
  public LinkDTO? ByID(Guid id) => _db.Links.SingleOrDefault(e => e.ID == id);
  public LinkDTO? ByName(string name) => _db.Links.SingleOrDefault(e => e.Name == name);

  public void Delete(Guid id) {
    if(ByID(id) is LinkDTO link) _db.Links.Remove(link);
  }

  public IEnumerable<LinkDTO> GetAll() => _db.Links;

  public void Save() {
    _db.SaveChanges();
  }

  public async Task SaveAsync() {
    await _db.SaveChangesAsync();
  }
}
