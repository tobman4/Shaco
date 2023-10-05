namespace Shaco.Contract.DTOs;

public class LinkDTO {
  public Guid ID { get; init; } = Guid.NewGuid();
  public string Name { get; init; } = null!;
  
  public Uri Uri { get; init; } = null!;

  public DateTime Created { get; init; } = DateTime.UtcNow;
}
