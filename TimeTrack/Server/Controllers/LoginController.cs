﻿using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TimeTrack.Server.Repositories;
using VM = TimeTrack.Shared.ViewModels;

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
        private readonly IUserRepository _userRepository;

        public LoginController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        [HttpPost]
        public async Task<ActionResult<VM.User>> Login(LoginRequest request)
        {
            var user = await _userRepository.Login(request.Email, request.Password);
            if (user is null)
            {
                return Unauthorized();
            }
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim("UserID", user.Id.ToString()),
            };
            var claimsIdentity = new ClaimsIdentity(
            claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                AllowRefresh = true,
                IsPersistent = true,
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
            return Ok(new VM.User()
            {
                Name = user.Name,
                Email = user.Email,
                Id = user.Id,
            });
        }

        [HttpDelete]
        public async Task<ActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
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
                Id = Convert.ToInt32(user.FindFirstValue("UserID")),
            };
            if (string.IsNullOrEmpty(viewUser.Email))
            {
                return Unauthorized();
            }
            return Ok(viewUser);
        }
    }
}
