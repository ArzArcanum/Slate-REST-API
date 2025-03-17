using Microsoft.EntityFrameworkCore;
using SlateAPI.Entities;

namespace SlateAPI.Persistence;

public class SlateDbContext : DbContext
{
    public SlateDbContext(DbContextOptions<SlateDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Message> Messages { get; set; }
}
