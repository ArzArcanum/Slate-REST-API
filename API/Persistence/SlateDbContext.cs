using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Persistence
{
    public class SlateDbContext : DbContext
    {
        private readonly IHostEnvironment _environment;

        public SlateDbContext(
            DbContextOptions<SlateDbContext> options,
            IHostEnvironment environment
        )
            : base(options)
        {
            _environment = environment;
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Message> Messages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            Console.WriteLine("OnModelCreating is executing...");
            if (_environment.IsDevelopment())
            {
                Console.WriteLine("Context in dev mode...");

                modelBuilder
                    .Entity<User>()
                    .HasData(
                        new User { Id = "TestUserId1", Username = "CryptoCrunk" },
                        new User { Id = "TestUserId2", Username = "Tonkatonk" },
                        new User { Id = "TestUserId3", Username = "JaquanTheJequel" }
                    );

                modelBuilder
                    .Entity<Message>()
                    .HasData(
                        new Message
                        {
                            Id = 1,
                            Content = "Hey everyone!",
                            CreatedAt = DateTime.UtcNow,
                            UserId = "TestUserId1",
                        },
                        new Message
                        {
                            Id = 2,
                            Content = "Hi",
                            CreatedAt = DateTime.UtcNow,
                            UserId = "TestUserId2",
                        },
                        new Message
                        {
                            Id = 3,
                            Content = "Yo",
                            CreatedAt = DateTime.UtcNow,
                            UserId = "TestUserId3",
                        },
                        new Message
                        {
                            Id = 4,
                            Content = "What's the plan tonight?",
                            CreatedAt = DateTime.UtcNow,
                            UserId = "TestUserId3",
                        }
                    );
            }
        }
    }
}
