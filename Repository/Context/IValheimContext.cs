using Microsoft.EntityFrameworkCore;
using api_valheim.models;

namespace api_valheim.Repository;

public interface IValheimContext
{
    public DbSet<Player> Players { get; set; }
    public DbSet<Character> Characters { get; set; }
    public DbSet<Item> Items { get; set; }

    public DbSet<User> Users { get; set; }

    public int SaveChanges();
}