using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using TimeTrack.Client.Services;

namespace TimeTrack.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            builder.Services.AddMudServices();
            builder.Services.AddAuthorizationCore();

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress + "api/") });
            builder.Services.AddScoped<AuthenticationStateProvider>(s => s.GetRequiredService<AuthProvider>());
            builder.Services.AddScoped<AuthProvider>();
            builder.Services.AddScoped<IAuth, Auth>();

            await builder.Build().RunAsync();
        }
    }
}