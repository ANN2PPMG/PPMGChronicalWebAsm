// Services/AdminAuthService.cs
namespace PPMGChronicalWebAsm.Services
{
    using Microsoft.AspNetCore.Components.Authorization;
    using Microsoft.JSInterop;
    using System.Threading.Tasks;

    public class AdminAuthService
    {
        private readonly IJSRuntime _jsRuntime;
        private const string AdminSessionKey = "admin_authenticated";

        // This would be better with secure hashing in a real app
        private const string AdminPassword = "Admin12345";
        public event Action? AuthenticationStateChanged;

        public AdminAuthService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
           
        }

        public async Task<bool> LoginAsync(string password)
        {
            bool isAuthenticated = password == AdminPassword;

            if (isAuthenticated)
            {
                await _jsRuntime.InvokeVoidAsync("sessionStorage.setItem", AdminSessionKey, "true");
                NotifyAuthenticationStateChanged();
            }

            return isAuthenticated;
        }

        public async Task<bool> IsAuthenticatedAsync()
        {
            var result = await _jsRuntime.InvokeAsync<string>("sessionStorage.getItem", AdminSessionKey);
            return result == "true";
        }

        public async Task LogoutAsync()
        {
            await _jsRuntime.InvokeVoidAsync("sessionStorage.removeItem", AdminSessionKey);
           NotifyAuthenticationStateChanged();
        }

        private void NotifyAuthenticationStateChanged()
        {
            AuthenticationStateChanged?.Invoke();
        }
    }
}
