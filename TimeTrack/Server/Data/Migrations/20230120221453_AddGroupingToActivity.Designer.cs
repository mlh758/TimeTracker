﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TimeTrack.Server.Data;

#nullable disable

namespace TimeTrack.Server.Data.Migrations
{
    [DbContext(typeof(TimeContext))]
    [Migration("20230120221453_AddGroupingToActivity")]
    partial class AddGroupingToActivity
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("ActivityAssessment", b =>
                {
                    b.Property<int>("AssessmentsId")
                        .HasColumnType("int");

                    b.Property<long>("ClientActivitiesId")
                        .HasColumnType("bigint");

                    b.HasKey("AssessmentsId", "ClientActivitiesId");

                    b.HasIndex("ClientActivitiesId");

                    b.ToTable("ActivityAssessment");
                });

            modelBuilder.Entity("CategoryClient", b =>
                {
                    b.Property<long>("DisabilitiesId")
                        .HasColumnType("bigint");

                    b.Property<long>("DisabledClientsId")
                        .HasColumnType("bigint");

                    b.HasKey("DisabilitiesId", "DisabledClientsId");

                    b.HasIndex("DisabledClientsId");

                    b.ToTable("CategoryClient");
                });

            modelBuilder.Entity("ClientGroup", b =>
                {
                    b.Property<long>("ClientsId")
                        .HasColumnType("bigint");

                    b.Property<long>("GroupsId")
                        .HasColumnType("bigint");

                    b.HasKey("ClientsId", "GroupsId");

                    b.HasIndex("GroupsId");

                    b.ToTable("ClientGroup");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("TimeTrack.Server.Models.Activity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<long>("ActivityGroupingId")
                        .HasColumnType("bigint");

                    b.Property<long?>("ClientId")
                        .HasColumnType("bigint");

                    b.Property<decimal>("ClinicalHours")
                        .HasPrecision(5, 2)
                        .HasColumnType("decimal(5,2)");

                    b.Property<bool>("ClinicallyScored")
                        .HasColumnType("bit");

                    b.Property<long?>("GroupId")
                        .HasColumnType("bigint");

                    b.Property<int?>("ScheduleId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Start")
                        .HasColumnType("date");

                    b.Property<bool>("UsedInIntegratedReport")
                        .HasColumnType("bit");

                    b.Property<bool>("UsedInResearch")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("ActivityGroupingId");

                    b.HasIndex("ClientId");

                    b.HasIndex("GroupId");

                    b.HasIndex("ScheduleId");

                    b.ToTable("Activities");
                });

            modelBuilder.Entity("TimeTrack.Server.Models.ActivityGrouping", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<int>("GroupingType")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("ActivityGrouping");
                });

            modelBuilder.Entity("TimeTrack.Server.Models.Assessment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasComment("Assessment names from buros.org mental measurements yearbook series");

                    b.HasKey("Id");

                    b.ToTable("Assessments");
                });

            modelBuilder.Entity("TimeTrack.Server.Models.Category", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("TimeTrack.Server.Models.Client", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<string>("Abbreviation")
                        .IsRequired()
                        .HasMaxLength(16)
                        .HasColumnType("nvarchar(16)");

                    b.Property<long>("AgeId")
                        .HasColumnType("bigint");

                    b.Property<long>("GenderId")
                        .HasColumnType("bigint");

                    b.Property<long>("RaceId")
                        .HasColumnType("bigint");

                    b.Property<long>("SettingId")
                        .HasColumnType("bigint");

                    b.Property<long>("SexualOrientationId")
                        .HasColumnType("bigint");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("AgeId");

                    b.HasIndex("GenderId");

                    b.HasIndex("RaceId");

                    b.HasIndex("SettingId");

                    b.HasIndex("SexualOrientationId");

                    b.HasIndex("UserId", "Abbreviation")
                        .IsUnique();

                    b.ToTable("Clients");
                });

            modelBuilder.Entity("TimeTrack.Server.Models.Group", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId", "Name")
                        .IsUnique();

                    b.ToTable("Groups");
                });

            modelBuilder.Entity("TimeTrack.Server.Models.Schedule", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<byte>("DaysOfWeek")
                        .HasColumnType("tinyint")
                        .HasComment("Bits for day of week Sunday to Saturday starting at the most significant bit.");

                    b.Property<DateTime>("EndSchedule")
                        .HasColumnType("date")
                        .HasComment("Date schedule should terminte on, inclusive");

                    b.Property<int>("Frequency")
                        .HasColumnType("int");

                    b.Property<int>("Interval")
                        .HasColumnType("int")
                        .HasComment("Gap between events. e.g 2 could be for every othe week");

                    b.HasKey("Id");

                    b.ToTable("Schedules");
                });

            modelBuilder.Entity("TimeTrack.Server.Models.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("TimeTrack.Server.Models.UserCredential", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<Guid>("AaGuid")
                        .HasColumnType("uniqueidentifier");

                    b.Property<byte[]>("CredentialId")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.Property<byte[]>("PublicKey")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<DateTime>("RegDate")
                        .HasColumnType("datetime2");

                    b.Property<long>("SignatureCounter")
                        .HasColumnType("bigint");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("UserCredentials");
                });

            modelBuilder.Entity("ActivityAssessment", b =>
                {
                    b.HasOne("TimeTrack.Server.Models.Assessment", null)
                        .WithMany()
                        .HasForeignKey("AssessmentsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TimeTrack.Server.Models.Activity", null)
                        .WithMany()
                        .HasForeignKey("ClientActivitiesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("CategoryClient", b =>
                {
                    b.HasOne("TimeTrack.Server.Models.Category", null)
                        .WithMany()
                        .HasForeignKey("DisabilitiesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TimeTrack.Server.Models.Client", null)
                        .WithMany()
                        .HasForeignKey("DisabledClientsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ClientGroup", b =>
                {
                    b.HasOne("TimeTrack.Server.Models.Client", null)
                        .WithMany()
                        .HasForeignKey("ClientsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TimeTrack.Server.Models.Group", null)
                        .WithMany()
                        .HasForeignKey("GroupsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("TimeTrack.Server.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("TimeTrack.Server.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TimeTrack.Server.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("TimeTrack.Server.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TimeTrack.Server.Models.Activity", b =>
                {
                    b.HasOne("TimeTrack.Server.Models.ActivityGrouping", "ActivityGrouping")
                        .WithMany("Activities")
                        .HasForeignKey("ActivityGroupingId")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();

                    b.HasOne("TimeTrack.Server.Models.Client", "Client")
                        .WithMany("Activities")
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TimeTrack.Server.Models.Group", "Group")
                        .WithMany("Activities")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.ClientCascade);

                    b.HasOne("TimeTrack.Server.Models.Schedule", "Schedule")
                        .WithMany()
                        .HasForeignKey("ScheduleId");

                    b.Navigation("ActivityGrouping");

                    b.Navigation("Client");

                    b.Navigation("Group");

                    b.Navigation("Schedule");
                });

            modelBuilder.Entity("TimeTrack.Server.Models.ActivityGrouping", b =>
                {
                    b.HasOne("TimeTrack.Server.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");

                    b.Navigation("User");
                });

            modelBuilder.Entity("TimeTrack.Server.Models.Category", b =>
                {
                    b.HasOne("TimeTrack.Server.Models.User", "User")
                        .WithMany("Categories")
                        .HasForeignKey("UserId");

                    b.Navigation("User");
                });

            modelBuilder.Entity("TimeTrack.Server.Models.Client", b =>
                {
                    b.HasOne("TimeTrack.Server.Models.Category", "Age")
                        .WithMany("AgedClients")
                        .HasForeignKey("AgeId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("TimeTrack.Server.Models.Category", "Gender")
                        .WithMany("GenderedClients")
                        .HasForeignKey("GenderId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("TimeTrack.Server.Models.Category", "Race")
                        .WithMany("RaceClients")
                        .HasForeignKey("RaceId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("TimeTrack.Server.Models.Category", "Setting")
                        .WithMany("SettingClients")
                        .HasForeignKey("SettingId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("TimeTrack.Server.Models.Category", "SexualOrientation")
                        .WithMany("SexualOrientationClients")
                        .HasForeignKey("SexualOrientationId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("TimeTrack.Server.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Age");

                    b.Navigation("Gender");

                    b.Navigation("Race");

                    b.Navigation("Setting");

                    b.Navigation("SexualOrientation");

                    b.Navigation("User");
                });

            modelBuilder.Entity("TimeTrack.Server.Models.Group", b =>
                {
                    b.HasOne("TimeTrack.Server.Models.User", "User")
                        .WithMany("Groups")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("TimeTrack.Server.Models.UserCredential", b =>
                {
                    b.HasOne("TimeTrack.Server.Models.User", "User")
                        .WithMany("Credentials")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("TimeTrack.Server.Models.ActivityGrouping", b =>
                {
                    b.Navigation("Activities");
                });

            modelBuilder.Entity("TimeTrack.Server.Models.Category", b =>
                {
                    b.Navigation("AgedClients");

                    b.Navigation("GenderedClients");

                    b.Navigation("RaceClients");

                    b.Navigation("SettingClients");

                    b.Navigation("SexualOrientationClients");
                });

            modelBuilder.Entity("TimeTrack.Server.Models.Client", b =>
                {
                    b.Navigation("Activities");
                });

            modelBuilder.Entity("TimeTrack.Server.Models.Group", b =>
                {
                    b.Navigation("Activities");
                });

            modelBuilder.Entity("TimeTrack.Server.Models.User", b =>
                {
                    b.Navigation("Categories");

                    b.Navigation("Credentials");

                    b.Navigation("Groups");
                });
#pragma warning restore 612, 618
        }
    }
}
