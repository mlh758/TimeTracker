using Microsoft.AspNetCore.Mvc;
using TimeTrack.Server.Models;
using TimeTrack.Server.Repositories;
using VM = TimeTrack.Shared.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace TimeTrack.Server.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    public class RegistrationController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public RegistrationController(UserManager<User> userManager,
            SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost]
        public async Task<ActionResult<VM.User>> Create(Shared.RegistrationForm form)
        {
            if (!ModelState.IsValid || form.Email is null)
            {
                return BadRequest(ModelState);
            }
            var user = new User() { Name = form.Name!, UserName = form.Email, Email = form.Email };
            var result = await _userManager.CreateAsync(user, form.Password);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);
                return Ok(new VM.User()
                {
                    Name = user.Name,
                    Email = user.Email,
                    Id = user.Id,
                });
            }
            else
            {
                return UnprocessableEntity(result);
            }

        }
    }
}
