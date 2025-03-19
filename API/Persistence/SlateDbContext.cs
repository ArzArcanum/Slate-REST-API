using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Persistence
{
    public class SlateDbContext : DbContext
    {
        public SlateDbContext(DbContextOptions<SlateDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Message> Messages { get; set; }
    }

}
