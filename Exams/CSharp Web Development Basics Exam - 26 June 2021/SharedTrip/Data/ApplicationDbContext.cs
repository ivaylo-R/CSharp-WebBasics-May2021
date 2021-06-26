namespace SharedTrip.Data
{
    using Microsoft.EntityFrameworkCore;
    using SharedTrip.Models;

    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; init; }
        public DbSet<Trip> Trips { get; init; }
        public DbSet<UserTrip> UserTrips { get; init; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseSqlServer(DatabaseConfiguration.ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserTrip>().HasKey(ut => new { ut.UserId, ut.TripId });

            modelBuilder.Entity<UserTrip>()
                .HasOne(ut => ut.User)
                .WithMany(s => s.UserTrips)
                .HasForeignKey(ut => ut.UserId);

            modelBuilder.Entity<UserTrip>()
                .HasOne(ut => ut.Trip)
                .WithMany(s => s.UserTrips)
                .HasForeignKey(ut => ut.TripId);
        }
    }
}
