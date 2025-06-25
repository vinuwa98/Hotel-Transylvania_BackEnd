using hms_backend.Models;
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
        


        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Room>().HasData(
        //        new Room
        //        {
        //            RoomType = "Luxury"
        //        },
        //        new Room
        //        {
        //            RoomType = "Normal"
        //        }
        //    );
        //}
    }
}
