using Microsoft.EntityFrameworkCore;
using Vinyl.Data.Entities;

namespace Vinyl.Data.Contexts
{
    public class VinylContext : DbContext
    {
        public VinylContext(DbContextOptions<VinylContext> options) : base(options) {}
        public DbSet<Artist> Artists { get; set; }
        public DbSet<Record> Records { get; set; }
        public DbSet<Genre> Genres { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Record>()
                .HasOne(r => r.Artist)
                .WithMany(a => a.Records);

            modelBuilder.Entity<Record>()
                .HasMany(r => r.Genres);

            modelBuilder.Entity<RecordGenre>()
                .HasKey(rg => new { rg.RecordId, rg.GenreId });

            modelBuilder.Entity<RecordGenre>()
                .HasOne(rg => rg.Record)
                .WithMany(rg => rg.Genres)
                .HasForeignKey(rg => rg.RecordId);

            modelBuilder.Entity<RecordGenre>()
                .HasOne(rg => rg.Genre)
                .WithMany(rg => rg.Records)
                .HasForeignKey(rg => rg.GenreId);
        }
    }
}
