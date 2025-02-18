namespace Shaco.API.Data.Models;

public class Link {

  public Guid ID { get; init; } = Guid.NewGuid();
  public string Name { get; init; } = null!;

  public string Url { get; set; } = null!;
}
