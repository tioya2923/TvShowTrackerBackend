using Microsoft.EntityFrameworkCore;
using TvShowTracker.Models;

namespace TvShowTracker.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Actor> Actors { get; set; }
        public DbSet<Favorite> Favorites { get; set; }
        public DbSet<TvShow> TvShows => Set<TvShow>();
        public DbSet<Episode> Episodes => Set<Episode>();
        public DbSet<Genre> Genres => Set<Genre>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Relação muitos-para-muitos entre TvShow e Actor
            modelBuilder.Entity<TvShow>()
                .HasMany(t => t.Actors)
                .WithMany(a => a.TvShows);

            // Relação muitos-para-muitos entre TvShow e Genre
            modelBuilder.Entity<TvShow>()
                .HasMany(t => t.Genres)
                .WithMany(g => g.TvShows);

            // Relação 1:N entre TvShow e Episode
            modelBuilder.Entity<Episode>()
                .HasOne(e => e.TvShow)
                .WithMany(t => t.Episodes)
                .HasForeignKey(e => e.TvShowId);

            // Relação 1:N entre TvShow e Favorite
            modelBuilder.Entity<Favorite>()
                .HasOne(f => f.TvShow)
                .WithMany(t => t.Favorites)
                .HasForeignKey(f => f.TvShowId);

            // Valor padrão para CreatedAt
            modelBuilder.Entity<TvShow>()
                .Property(t => t.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
        }
    }
}
