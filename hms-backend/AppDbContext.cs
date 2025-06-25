using HmsBackend.Models;
using HmsBackend.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HmsBackend
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext(options)
    {
        public DbSet<Room> Rooms { get; set; }
        public DbSet<User> Accounts { get; set; }

        public DbSet<Complaint> Complaint => Set<Complaint>();
        public DbSet<Job> Job => Set<Job>();



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Job>()
                .HasOne(j => j.CreatedUser)
                .WithMany()
                .HasForeignKey(j => j.CreatedUserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Job>()
                .HasOne(j => j.AssignedManagereUser)
                .WithMany()
                .HasForeignKey(j => j.AssignedManagerUserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<JobUser>()
      .HasKey(ju => new { ju.JobId, ju.UserId });

            modelBuilder.Entity<JobUser>()
                .HasOne(ju => ju.Job)
                .WithMany(j => j.JobUsers)
                .HasForeignKey(ju => ju.JobId);

            modelBuilder.Entity<JobUser>()
                .HasOne(ju => ju.User)
                .WithMany(u => u.JobUsers)
                .HasForeignKey(ju => ju.UserId);
        }


    }
}
