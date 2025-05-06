using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace PPMGChronicalWebAsm.Services
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly AdminAuthService _adminAuthService;

        public CustomAuthStateProvider(AdminAuthService adminAuthService)
        {
            _adminAuthService = adminAuthService;
            _adminAuthService.AuthenticationStateChanged += NotifyAuthenticationStateChanged;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var isAuthenticated = await _adminAuthService.IsAuthenticatedAsync();

            if (isAuthenticated)
            {
                var identity = new ClaimsIdentity(new[]
                {
                new Claim(ClaimTypes.Name, "Admin")
            }, "AdminAuth");

                var user = new ClaimsPrincipal(identity);
                return new AuthenticationState(user);
            }

            // Return anonymous user
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }

        public void NotifyAuthenticationStateChanged()
        {
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
    }
}
