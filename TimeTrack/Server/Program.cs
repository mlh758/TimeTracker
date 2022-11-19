using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using TimeTrack.Server.Repositories;
using TimeTrack.Server.Models;
using Microsoft.AspNetCore.Identity;

namespace TimeTrack
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();
            var dbConnectionString = builder.Configuration.GetConnectionString("TimeTrack");
            builder.Services.AddDbContext<Server.Data.TimeContext>(options =>
                options.UseSqlServer(dbConnectionString));
            builder.Services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<Server.Data.TimeContext>();
            builder.Services.AddScoped<IActivityRepository, ActivityRepository>();
            builder.Services.AddScoped<IAssessmentRepository, AssessmentRepository>();
            builder.Services.AddScoped<IClientRepository, ClientRepository>();

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.ExpireTimeSpan = TimeSpan.FromHours(8);
                    options.SlidingExpiration = true;
                    options.AccessDeniedPath = "/Forbidden/";
                });
            builder.Services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequiredLength = 12;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;
                options.User.RequireUniqueEmail = true;
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseWebAssemblyDebugging();
                Server.Data.Seed.BasicData.Seed(dbConnectionString);

            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseCookiePolicy();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapRazorPages();
            app.MapControllers();
            app.MapFallbackToFile("index.html");

            app.Run();
        }
    }
}
