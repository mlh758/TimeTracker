using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using TimeTrack.Shared;
using TimeTrack.Shared.ViewModels;

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
                return new AuthenticationState(BuildPrinciple(serverUser));
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
            var user = await _auth.Register(form);
            var authState = new AuthenticationState(BuildPrinciple(user));
            NotifyAuthenticationStateChanged(Task.FromResult(authState));
        }

        public async Task Logout()
        {
            await _auth.Logout();
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        private ClaimsPrincipal BuildPrinciple(User user)
        {
            var identity = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                }, "Check cookie auth type");

            return new ClaimsPrincipal(identity);
        }
    }
}
