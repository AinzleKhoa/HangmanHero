using Microsoft.EntityFrameworkCore;
using g1_hangmanhero.Models;

namespace g1_hangmanhero.Data
{
    public class HangmanHeroContext : DbContext
    {
        public DbSet<Player> Players { get; set; }
        public DbSet<Word> Words { get; set; }
        public DbSet<GameHistory> GameHistories { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Replace the connection string if needed
            optionsBuilder.UseSqlServer("Server=localhost;Database=HangmanHero;Integrated Security=True;TrustServerCertificate=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Optional: Apply constraints or default values
            modelBuilder.Entity<Player>()
                .HasIndex(p => p.Username)
                .IsUnique();
        }
    }
}
