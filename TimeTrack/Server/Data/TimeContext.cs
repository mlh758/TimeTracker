using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using TimeTrack.Shared.Models;

namespace TimeTrack.Server.Data
{
    public partial class TimeContext : DbContext
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
        public DbSet<Shared.Models.Client> Clients { get; set; }
        public DbSet<Activity> Activities { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Shared.Models.Client>().HasMany(c => c.Disabilities).WithMany(c => c.DisabledClients);
            modelBuilder.Entity<Category>().HasMany(c => c.RaceClients).WithOne(c => c.Race).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Category>().HasMany(c => c.GenderedClients).WithOne(c => c.Gender).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Category>().HasMany(c => c.SettingClients).WithOne(c => c.Setting).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Category>().HasMany(c => c.SexualOrientationClients).WithOne(c => c.SexualOrientation).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Category>().HasMany(c => c.AgedClients).WithOne(c => c.Age).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
