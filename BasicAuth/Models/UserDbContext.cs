using Microsoft.EntityFrameworkCore;

namespace BasicAuth.Models
{
    public class UserDbContext : DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
                new User() { Id = 1, FirstName = "Laxmi Kanth", LastName = "Munagala", Email = "laxmikanth@gmail.com", Password = "passwrod123" },
                new User() { Id = 2, FirstName = "Gayatri", LastName = "Shinde", Email = "gayatri@gmail.com", Password = "passwrod123" }
                );
        }
    }
}
