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
        public DbSet<ClientActivity> ClientActivities { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
