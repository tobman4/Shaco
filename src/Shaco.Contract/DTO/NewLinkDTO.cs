namespace Shaco.Contract.DTOs;

public class NewLinkDTO {
  public string? Name { get; init; } = null;
  public Uri Dest { get; init; } = null!;
}
