using HmsBackend.Models;
using HmsBackend.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace HmsBackend
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext(options)
    {
        public DbSet<Room> Rooms { get; set; }
        

        public DbSet<Complaint> Complaint => Set<Complaint>();
        public DbSet<Job> Job => Set<Job>();



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Job>()
                .HasOne(j => j.Complaint)
                .WithMany(c => c.Jobs)
                .HasForeignKey(j => j.ComplaintId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Complaint>()
                .HasOne(c => c.User)
                .WithMany(u => u.Complaints)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Complaint>()
                .HasOne(c => c.Room)
                .WithMany(r => r.Complaints)
                .HasForeignKey(c => c.RoomId)
                .OnDelete(DeleteBehavior.Restrict);



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
