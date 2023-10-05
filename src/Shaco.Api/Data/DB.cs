using Microsoft.EntityFrameworkCore;
using Shaco.Contract.DTOs;

namespace Shaco.Api.Data;

public class DB : DbContext {

  private readonly IConfiguration _config;

  public DbSet<LinkDTO> Links { get; set; } = null!;

  public DB(IConfiguration config) {
    _config = config;
  }

  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
    var dbFile = _config.GetValue<string>("DB");
    if(string.IsNullOrWhiteSpace(dbFile)) throw new Exception("Missing db path");

    optionsBuilder.UseSqlite($"Data Source={dbFile}");
  }

  protected override void OnModelCreating(ModelBuilder modelBuilder) { 
    modelBuilder.Entity<LinkDTO>()
      .HasKey(e => e.ID);
  }

}
