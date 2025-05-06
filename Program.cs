using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using PPMGChronicalWebAsm.Services;


namespace PPMGChronicalWebAsm
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);

            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            var baseAddress = builder.HostEnvironment.BaseAddress;
            if (baseAddress.EndsWith("/"))
            {
                baseAddress = baseAddress.TrimEnd('/') + "/PPMGChronicalWebAsm/"; // Replace with your repository name
            }

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            builder.Services.AddScoped<SqliteService>();
            builder.Services.AddSingleton<ErrorService>();

            // Add authentication services
            builder.Services.AddAuthorizationCore();
            builder.Services.AddScoped<AdminAuthService>();
            builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();



            await builder.Build().RunAsync();
        }
    }
}
