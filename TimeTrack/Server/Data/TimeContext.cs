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
        public DbSet<CustomCategory> CustomCategories { get; set; }
        public DbSet<Schedule> Schedules { get; set; }

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

            modelBuilder.Entity<Models.Client>().HasMany(c => c.Disabilities).WithMany(c => c.DisabledClients);
            modelBuilder.Entity<Category>().HasMany(c => c.RaceClients).WithOne(c => c.Race).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Category>().HasMany(c => c.GenderedClients).WithOne(c => c.Gender).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Category>().HasMany(c => c.SettingClients).WithOne(c => c.Setting).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Category>().HasMany(c => c.SexualOrientationClients).WithOne(c => c.SexualOrientation).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Category>().HasMany(c => c.AgedClients).WithOne(c => c.Age).OnDelete(DeleteBehavior.Restrict);

            // same thing again for custom categories
            modelBuilder.Entity<Models.Client>()
                .HasMany(c => c.CustomDisabilities)
                .WithMany(c => c.DisabledClients)
                .UsingEntity<ClientCustomDisability>(
                j => j
                    .HasOne(pt => pt.Disability)
                    .WithMany(t => t.ClientCustomDisabilities)
                    .HasForeignKey(pt => pt.DisabilityId).OnDelete(DeleteBehavior.Restrict),
                j => j
                    .HasOne(pt => pt.Client)
                    .WithMany(p => p.ClientCustomDisabilities)
                    .HasForeignKey(pt => pt.ClientId).OnDelete(DeleteBehavior.Cascade),
                j =>
                {
                    j.HasKey(t => new { t.DisabilityId, t.ClientId });
                });
            modelBuilder.Entity<CustomCategory>().HasMany(c => c.RaceClients).WithOne(c => c.CustomRace).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<CustomCategory>().HasMany(c => c.GenderedClients).WithOne(c => c.CustomGender).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<CustomCategory>().HasMany(c => c.SettingClients).WithOne(c => c.CustomSetting).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<CustomCategory>().HasMany(c => c.SexualOrientationClients).WithOne(c => c.CustomSexualOrientation).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<CustomCategory>().HasMany(c => c.AgedClients).WithOne(c => c.CustomAge).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
