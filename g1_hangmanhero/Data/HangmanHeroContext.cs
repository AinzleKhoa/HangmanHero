using g1_hangmanhero.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace HangmanDemo.Models
{
    public partial class HangmanHeroContext : DbContext
    {
        public HangmanHeroContext() { }

        public HangmanHeroContext(DbContextOptions<HangmanHeroContext> options)
            : base(options) { }

        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<GameHistory> GameHistories { get; set; }
        public virtual DbSet<Player> Players { get; set; }
        public virtual DbSet<Room> Rooms { get; set; }
        public virtual DbSet<Use> Uses { get; set; }
        public virtual DbSet<Word> Words { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured) // ✅ tránh cấu hình trùng
            {
                var configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsetting.json")
                    .Build();

                var connectionString = configuration.GetConnectionString("DBDefault");
                optionsBuilder.UseSqlServer(connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // ✅ CATEGORY
            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.CategoryId);
                entity.ToTable("CATEGORY");

                entity.Property(e => e.CategoryId)
                      .ValueGeneratedNever()
                      .HasColumnName("Category_ID");

                entity.Property(e => e.CategoryName)
                      .HasMaxLength(20)
                      .HasColumnName("Category_Name");

                entity.Property(e => e.NumberOfSeat)
                      .HasColumnName("Number_Of_Seat");
            });

            // ✅ CUSTOMER
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasKey(e => e.CustomerId);
                entity.ToTable("CUSTOMER");

                entity.Property(e => e.CustomerId)
                      .ValueGeneratedNever()
                      .HasColumnName("Customer_ID");

                entity.Property(e => e.CustomerName)
                      .HasMaxLength(20)
                      .HasColumnName("Customer_Name");

                entity.Property(e => e.Note).HasMaxLength(50);
                entity.Property(e => e.Phone).HasMaxLength(10);
            });

            // ✅ GAME HISTORIES (Quan hệ với Player & Word)
            modelBuilder.Entity<GameHistory>(entity =>
            {
                entity.HasKey(e => e.GameId);

                entity.Property(e => e.PlayedAt)
                      .HasDefaultValueSql("(getdate())")
                      .HasColumnType("datetime");

                entity.HasOne(d => d.Player)
                      .WithMany(p => p.GameHistories)
                      .HasForeignKey(d => d.PlayerId)
                      .HasConstraintName("FK_GameHistories_Players");

                entity.HasOne(d => d.Word)
                      .WithMany(p => p.GameHistories)
                      .HasForeignKey(d => d.WordId)
                      .HasConstraintName("FK_GameHistories_Words");
            });

            // ✅ PLAYER (Quan hệ 1-Nhiều với GameHistories)
            modelBuilder.Entity<Player>(entity =>
            {
                entity.HasKey(e => e.PlayerId);

                entity.HasIndex(e => e.Username)
                      .IsUnique();

                entity.Property(e => e.Username).HasMaxLength(50);
                entity.Property(e => e.PasswordHash).HasMaxLength(255);
                entity.Property(e => e.DefaultDifficulty).HasMaxLength(10);

                entity.Property(e => e.JoinDate)
                      .HasDefaultValueSql("(getdate())")
                      .HasColumnType("datetime");
            });

            // ✅ ROOM
            modelBuilder.Entity<Room>(entity =>
            {
                entity.HasKey(e => e.RoomId);
                entity.ToTable("ROOM");

                entity.Property(e => e.RoomId)
                      .ValueGeneratedNever()
                      .HasColumnName("Room_ID");

                entity.Property(e => e.CategoryId)
                      .HasColumnName("Category_ID");

                entity.HasOne(d => d.Category)
                      .WithMany(p => p.Rooms)
                      .HasForeignKey(d => d.CategoryId)
                      .HasConstraintName("FK__ROOM__Category_I__3A81B327");
            });

            // ✅ USE (Quan hệ nhiều-nhiều Customer - Room)
            modelBuilder.Entity<Use>(entity =>
            {
                entity.HasKey(e => new { e.CustomerId, e.RoomId });
                entity.ToTable("Use");

                entity.Property(e => e.CustomerId).HasColumnName("Customer_ID");
                entity.Property(e => e.RoomId).HasColumnName("Room_ID");
                entity.Property(e => e.StartTime).HasColumnType("datetime");
                entity.Property(e => e.EndTime).HasColumnType("datetime");

                entity.HasOne(d => d.Customer)
                      .WithMany(p => p.Uses)
                      .HasForeignKey(d => d.CustomerId)
                      .OnDelete(DeleteBehavior.ClientSetNull)
                      .HasConstraintName("FK__Use__Customer_ID__3D5E1FD2");

                entity.HasOne(d => d.Room)
                      .WithMany(p => p.Uses)
                      .HasForeignKey(d => d.RoomId)
                      .OnDelete(DeleteBehavior.ClientSetNull)
                      .HasConstraintName("FK__Use__Room_ID__3E52440B");
            });

            // ✅ WORD
            modelBuilder.Entity<Word>(entity =>
            {
                entity.HasKey(e => e.WordId);

                entity.Property(e => e.Text).HasMaxLength(100);
                entity.Property(e => e.Category).HasMaxLength(50);
                entity.Property(e => e.Difficulty).HasMaxLength(10);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
