using Shaco.Contract.DTOs;

namespace Shaco.Contract.Interfaces;

public interface ILinkRepo {

  IEnumerable<LinkDTO> All => GetAll().ToArray();

  IEnumerable<LinkDTO> GetAll();

  LinkDTO Add(LinkDTO data);

  LinkDTO? ByID(Guid id);
  LinkDTO? ByName(string name);

  LinkDTO? GetOne(string str) {
    Guid id;
    if(Guid.TryParse(str, out id)) return ByID(id);
    else return ByName(str);
  }

  void Delete(Guid id);

  void Save();
  Task SaveAsync();
}
