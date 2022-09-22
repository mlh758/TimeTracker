using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using TimeTrack.Shared;

namespace TimeTrack.Client.Services
{
    class AuthProvider : AuthenticationStateProvider
    {
        private readonly IAuth _auth;
        public AuthProvider(IAuth auth)
        {
            _auth = auth;
        }
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
  
            
            try
            {
                var serverUser = await _auth.CurrentUser();
                var identity = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, serverUser.Name),
                    new Claim(ClaimTypes.Email, serverUser.Email),
                    new Claim("UserID", serverUser.Id.ToString()),
                }, "Check cookie auth type");

                var principal = new ClaimsPrincipal(identity);
                return new AuthenticationState(principal);
            } catch(HttpRequestException)
            {
                return new AuthenticationState(new ClaimsPrincipal());
            }
        }

        public async Task Login(LoginForm form)
        {
            await _auth.Login(form);
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public async Task Register(RegistrationForm form)
        {
            await _auth.Register(form);
        }
    }
}
