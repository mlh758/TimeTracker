using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using VM = TimeTrack.Shared.ViewModels;
using M = TimeTrack.Server.Models;
using Microsoft.AspNetCore.Identity;
using Fido2NetLib;
using TimeTrack.Server.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading;

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
        private readonly TimeContext _context;
        private readonly IFido2 _fido;

        public LoginController(SignInManager<M.User> signInManager, TimeContext context, IFido2 fido)
        {
            _signIn = signInManager;
            _context = context;
            _fido = fido;
        }
        [HttpPost]
        public async Task<ActionResult<VM.User>> Login(LoginRequest request)
        {
            var user = await _signIn.UserManager.FindByEmailAsync(request.Email);
            if (user is null)
            {
                return NotFound();
            }
            var result = await _signIn.PasswordSignInAsync(user, request.Password, true, true);
            if (result.Succeeded)
            {
                return Ok(ConvertUser(user));
            }
            else
            {
                return Forbid("Bearer");
            }

        }

        [HttpPost("fido")]
        public async Task<ActionResult<VM.User>> FidoLogin(AuthenticatorAssertionRawResponse clientResponse)
        {
            var jsonOptions = HttpContext.Session.GetString(AuthenticatorsController.sessionOptionsKey);
            var options = AssertionOptions.FromJson(jsonOptions);
            var savedCredential = await _context.UserCredentials.Where(c => c.CredentialId == clientResponse.Id).Include(c => c.User).FirstAsync();
            IsUserHandleOwnerOfCredentialIdAsync callback =  (args, cancellationToken) =>
            {
                return Task.FromResult(true);
            };
            var res = await _fido.MakeAssertionAsync(clientResponse, options, savedCredential.PublicKey, savedCredential.SignatureCounter, callback);
            await _signIn.SignInAsync(savedCredential.User!, true, "WebAuthn");
            return Ok(ConvertUser(savedCredential.User!));
        }

        private VM.User ConvertUser(M.User user)
        {
            return new VM.User()
            {
                Name = user.Name,
                Email = user.Email,
                Id = user.Id,
            };
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
