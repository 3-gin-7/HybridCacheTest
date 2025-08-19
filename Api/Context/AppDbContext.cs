using Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Context;

public class AppDbContext : DbContext
{
    public DbSet<Data> DataEntries { get; set; } = null!;
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    => optionsBuilder.UseSqlite("Data Source=app.db");
}