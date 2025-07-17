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
            optionsBuilder.UseSqlServer("server=.;Database=HangmanHero;uid=sa;pwd=123456;Trust Server Certificate=True;");
        }
    }
}
