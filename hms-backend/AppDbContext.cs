using HmsBackend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace HmsBackend
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<User>(options)
    {
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Complaint> Complaint => Set<Complaint>();
        public DbSet<Job> Job => Set<Job>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.Ignore(u => u.EmailConfirmed);
                entity.Ignore(u => u.PhoneNumberConfirmed);
                entity.Ignore(u => u.TwoFactorEnabled);
                entity.Ignore(u => u.LockoutEnabled);
                entity.Ignore(u => u.AccessFailedCount);
                entity.Ignore(u => u.ConcurrencyStamp);
                entity.Ignore(u => u.SecurityStamp);
                entity.Ignore(u => u.LockoutEnd);
            });

            modelBuilder.Entity<Job>()
                .HasOne(j => j.Complaint)
                .WithMany(c => c.Jobs)
                .HasForeignKey(j => j.ComplaintId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete

            // Configure Complaint -> User relationship (many Complaints to one User)
            modelBuilder.Entity<Complaint>()
                .HasOne(c => c.User)
                .WithMany(u => u.Complaints)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete

            // Configure Complaint -> Room relationship (many Complaints to one Room)
            modelBuilder.Entity<Complaint>()
                .HasOne(c => c.Room)
                .WithMany(r => r.Complaints)
                .HasForeignKey(c => c.RoomId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete

            // Configure Job -> CreatedUser relationship (one User creates many Jobs)
            modelBuilder.Entity<Job>()
                .HasOne(j => j.CreatedUser)
                .WithMany()
                .HasForeignKey(j => j.CreatedUserId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete

            // Configure Job -> AssignedManagerUser relationship (one User manages many Jobs)
            modelBuilder.Entity<Job>()
                .HasOne(j => j.AssignedManagereUser)
                .WithMany()
                .HasForeignKey(j => j.AssignedManagerUserId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete

            // Configure composite key for JobUser (many-to-many between Job and User)
            modelBuilder.Entity<JobUser>()
                .HasKey(ju => new { ju.JobId, ju.UserId });

            // Configure JobUser -> Job relationship (many JobUsers to one Job)
            modelBuilder.Entity<JobUser>()
                .HasOne(ju => ju.Job)
                .WithMany(j => j.JobUsers)
                .HasForeignKey(ju => ju.JobId);

            // Configure JobUser -> User relationship (many JobUsers to one User)
            modelBuilder.Entity<JobUser>()
                .HasOne(ju => ju.User)
                .WithMany(u => u.JobUsers)
                .HasForeignKey(ju => ju.UserId);
        }
    }
}
