using Microsoft.EntityFrameworkCore;
using api_valheim.models;

namespace api_valheim.Repository;


public class ValheimContext : DbContext, IValheimContext
{
    public ValheimContext(DbContextOptions<ValheimContext> options) : base(options) { }

    public DbSet<Player> Players { get; set; } = null!;
    public DbSet<Character> Characters { get; set; } = null!;
    public DbSet<Item> Items { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var connectionString = "Server=127.0.0.1;Database=ValheimDB;User=SA;Password=ValheimDB!;TrustServerCertificate=True";

            optionsBuilder.UseSqlServer(connectionString);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Player>()
            .HasMany(p => p.Characters)
            .WithOne(c => c.Player)
            .HasForeignKey(c => c.PlayerId);

        modelBuilder.Entity<Item>()
            .HasOne(c => c.Character)
            .WithMany(i => i.Items)
            .HasForeignKey(i => i.CharacterId);
    }
}