using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TimeTrack.Server.Repositories;
using VM = TimeTrack.Shared.ViewModels;
using M = TimeTrack.Server.Models;
using Microsoft.AspNetCore.Identity;

namespace TimeTrack.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        public readonly struct LoginRequest
        {
            public string Email { get; init; }
            public string Password { get; init; }
        }
        private readonly SignInManager<M.User> _signIn;

        public LoginController(SignInManager<M.User> signInManager)
        {
            _signIn = signInManager;
        }
        [HttpPost]
        public async Task<ActionResult<VM.User>> Login(LoginRequest request)
        {
            var user = await _signIn.UserManager.FindByEmailAsync(request.Email);
            var result = await _signIn.PasswordSignInAsync(user, request.Password, true, true);
            if (result.Succeeded)
            {
                return Ok(new VM.User()
                {
                    Name = user.Name,
                    Email = user.Email,
                    Id = user.Id,
                });
            }
            else
            {
                return Forbid();
            }

        }

        [HttpDelete]
        public async Task<ActionResult> Logout()
        {
            await _signIn.SignOutAsync();
            return Ok();
        }

        [HttpGet]
        public ActionResult<VM.User> Login()
        {
            var user = HttpContext.User;
            if (user is null)
            {
                return Unauthorized();
            }
            var viewUser = new VM.User()
            {
                Name = user.FindFirstValue(ClaimTypes.Name),
                Email = user.FindFirstValue(ClaimTypes.Email),
                Id = user.FindFirstValue(ClaimTypes.NameIdentifier),
            };
            if (string.IsNullOrEmpty(viewUser.Email))
            {
                return Unauthorized();
            }
            return Ok(viewUser);
        }
    }
}
