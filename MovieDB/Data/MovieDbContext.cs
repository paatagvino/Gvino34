using Microsoft.EntityFrameworkCore;
using MovieDB.Entities;

namespace MovieDB.Data
{
    public class MovieDbContext : DbContext
    {
        // DbSets

        public DbSet<Country> Countries { get; set; }

        public DbSet<Studio> Studios { get; set; }

        public DbSet<StudioDetails> StudioDetails { get; set; }

        public DbSet<Movie> Movies { get; set; }

        public DbSet<Actor> Actors { get; set; }


        // SQL Server Connection

        protected override void OnConfiguring(
    DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                @"Server=Gv1no;Database=MovieDB;Trusted_Connection=True;TrustServerCertificate=True;");
        }


        // Fluent API Configuration

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            // 1. Country - Studio (One-to-Many)

            modelBuilder.Entity<Studio>()
                .HasOne(s => s.Country)
                .WithMany(c => c.Studios)
                .HasForeignKey(s => s.CountryId);


            // 2. Studio - StudioDetails (One-to-One)

            modelBuilder.Entity<Studio>()
                .HasOne(s => s.StudioDetails)
                .WithOne(sd => sd.Studio)
                .HasForeignKey<StudioDetails>(sd => sd.StudioId);


            // 3. Studio - Movie (One-to-Many)

            modelBuilder.Entity<Movie>()
                .HasOne(m => m.Studio)
                .WithMany(s => s.Movies)
                .HasForeignKey(m => m.StudioId);


            // 4. Movie - Actor (Many-to-Many)

            modelBuilder.Entity<Movie>()
                .HasMany(m => m.Actors)
                .WithMany(a => a.Movies)
                .UsingEntity<Dictionary<string, object>>(
                    "MovieActors",

                    j => j
                        .HasOne<Actor>()
                        .WithMany()
                        .HasForeignKey("ActorId"),

                    j => j
                        .HasOne<Movie>()
                        .WithMany()
                        .HasForeignKey("MovieId"),

                    j =>
                    {
                        j.HasKey("MovieId", "ActorId");
                    });


            // Required fields and MaxLength

            modelBuilder.Entity<Country>()
                .Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100);


            modelBuilder.Entity<Studio>()
                .Property(s => s.Name)
                .IsRequired()
                .HasMaxLength(100);


            modelBuilder.Entity<StudioDetails>()
                .Property(sd => sd.LicenseNumber)
                .IsRequired();


            modelBuilder.Entity<Movie>()
                .Property(m => m.Title)
                .IsRequired()
                .HasMaxLength(150);


            modelBuilder.Entity<Actor>()
                .Property(a => a.FirstName)
                .IsRequired()
                .HasMaxLength(100);


            modelBuilder.Entity<Actor>()
                .Property(a => a.LastName)
                .IsRequired()
                .HasMaxLength(100);
        }
    }
}