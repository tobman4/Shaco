using Shaco.API.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Shaco.API.Data;

public abstract class DB : DbContext {

  public DbSet<Link> Links { get; init; } = null!;

  protected override void OnModelCreating(ModelBuilder builder) {
    builder.Entity<Link>()
      .HasKey(e => e.ID);

    builder.Entity<Link>()
      .HasAlternateKey(e => e.Name);
  }

}
