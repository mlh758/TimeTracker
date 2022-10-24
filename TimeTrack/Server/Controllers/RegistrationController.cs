using Microsoft.AspNetCore.Mvc;
using TimeTrack.Shared.Models;
using TimeTrack.Server.Repositories;
using VM = TimeTrack.Shared.ViewModels;

namespace TimeTrack.Server.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    public class RegistrationController : Controller
    {
        private readonly IUserRepository _userRepository;

        public RegistrationController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpPost]
        public async Task<ActionResult<VM.User>> Create(Shared.RegistrationForm form)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = new User(form.Name!, form.Email!, form.Password!);
            var savedUser = await _userRepository.Create(user);
            return Ok(new VM.User()
            {
                Name = savedUser.Name,
                Email = savedUser.Email,
                Id = savedUser.Id,
            });
        }
    }
}
