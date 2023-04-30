using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Hosting;
using TimeTrack.Server.Models;


namespace TimeTrack.Server.Data
{
    public partial class TimeContext : IdentityDbContext<User>
    {
        private readonly string _connectionString;

        public TimeContext()
        {
        }

        public TimeContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public TimeContext(DbContextOptions<TimeContext> options)
            : base(options)
        {
        }

        public DbSet<Assessment> Assessments { get; set; }
        public DbSet<Models.Client> Clients { get; set; }
        public DbSet<Activity> Activities { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<UserCredential> UserCredentials { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<ActivityGrouping> ActivityGrouping { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Activity>().HasOne(c => c.Client).WithMany(c => c.Activities).HasForeignKey(c => c.ClientId).OnDelete(DeleteBehavior.ClientCascade);
            modelBuilder.Entity<Activity>().HasOne(c => c.Group).WithMany(c => c.Activities).HasForeignKey(c => c.GroupId).OnDelete(DeleteBehavior.ClientCascade);
            modelBuilder.Entity<Activity>().HasOne(c => c.ActivityGrouping).WithMany(c => c.Activities).HasForeignKey(c => c.ActivityGroupingId).OnDelete(DeleteBehavior.ClientCascade);
            modelBuilder.Entity<Activity>().HasOne(c => c.User).WithMany(u => u.Activities).HasForeignKey(c => c.UserId).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Group>().HasOne(c => c.User).WithMany(u => u.Groups).HasForeignKey(c => c.UserId).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Models.Client>().HasMany(c => c.Disabilities).WithMany(c => c.DisabledClients);
            modelBuilder.Entity<Category>().HasMany(c => c.RaceClients).WithOne(c => c.Race).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Category>().HasMany(c => c.GenderedClients).WithOne(c => c.Gender).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Category>().HasMany(c => c.SettingClients).WithOne(c => c.Setting).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Category>().HasMany(c => c.SexualOrientationClients).WithOne(c => c.SexualOrientation).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Category>().HasMany(c => c.AgedClients).WithOne(c => c.Age).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
