using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Fido2NetLib;
using Fido2NetLib.Objects;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using TimeTrack.Server.Models;
using System.Text;
using Microsoft.Extensions.Options;
using TimeTrack.Server.Data;
using Microsoft.EntityFrameworkCore;
using TimeTrack.Shared.ViewModels;

namespace TimeTrack.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    /*
     * When working on this, you could end up creating a LOT of credentials.
     * This would be undesirable as a permanent thing, use the Webauthn dev tools:
     * https://developer.chrome.com/docs/devtools/webauthn/
     */
    public class AuthenticatorsController : Controller
    {
        public readonly struct CreateCredentialRequest
        {
            public string Name { get; init; }
            public AuthenticatorAttestationRawResponse Credential { get; init; }
        }
        public class CredentialCreateException : ApplicationException { }
        private readonly TimeContext _context;
        private readonly IFido2 _fido;
        public static readonly string attestationOptionsKey = "fido2.attestationOptions";
        public static readonly string sessionOptionsKey = "fido2.assertionOptions";

        public AuthenticatorsController(IFido2 fido, TimeContext context)
        {
            _fido = fido;
            _context = context;
        }
        [HttpGet("new")]
        public async Task<ActionResult> Config()
        {
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var dbUser = await _context.Users.Where(u => u.Id == userId).Include(u => u.Credentials).FirstOrDefaultAsync();
            if (dbUser is null)
            {
                throw new ArgumentNullException(nameof(dbUser));
            }
            var user = new Fido2User { DisplayName = dbUser.Name, Id = Encoding.ASCII.GetBytes(dbUser.Id), Name = dbUser.Email, };
            var options = _fido.RequestNewCredential(user, dbUser.Credentials!.Select(c => new PublicKeyCredentialDescriptor(c.CredentialId)).ToList());
            HttpContext.Session.SetString(attestationOptionsKey, options.ToJson());
            return Json(options);
        }

        [HttpGet("options")]
        [AllowAnonymous]
        public async Task<ActionResult> AssertionOptions(string username)
        {
            var dbUser = await _context.Users.Where(u => u.Email == username).Include(u => u.Credentials).FirstOrDefaultAsync();
            if (dbUser is null)
            {
                return NotFound();
            }
            var existingCredentials = dbUser.Credentials!.Select(c => new PublicKeyCredentialDescriptor(c.CredentialId)).ToList();
            var options = _fido.GetAssertionOptions(existingCredentials, UserVerificationRequirement.Preferred);
            HttpContext.Session.SetString(sessionOptionsKey, options.ToJson());
            return Json(options);
        }

        [HttpPost]
        public async Task<ActionResult> Create(CreateCredentialRequest request)
        {
            var jsonOptions = HttpContext.Session.GetString(attestationOptionsKey);
            HttpContext.Session.Remove(attestationOptionsKey);
            var options = CredentialCreateOptions.FromJson(jsonOptions);
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            IsCredentialIdUniqueToUserAsyncDelegate callback = async (args, cancellationToken) =>
            {
                var count = await _context.UserCredentials.Where(uc => uc.UserId == userId).CountAsync();
                return count <= 1;
            };
            var success = await _fido.MakeNewCredentialAsync(request.Credential, options, callback);
            if (success.Result is null)
            {
                throw new CredentialCreateException();
            }
            var newCredential = new UserCredential(userId, success.Result.PublicKey, success.Result.CredentialId)
            {
                SignatureCounter = success.Result.Counter,
                RegDate = DateTime.Now,
                AaGuid = success.Result.Aaguid,
                Name = request.Name,
            };
            _context.UserCredentials.Add(newCredential);
            await _context.SaveChangesAsync();
            return Created(nameof(Show), newCredential);
        }

        [HttpGet("{credId}")]
        public async Task<ActionResult<UserCredential>> Show(int credId)
        {
            var credential = await Find(credId);
            if (credential is null)
            {
                return NotFound();
            }
            return credential;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Authenticator>>> Index()
        {
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            return await _context.UserCredentials.Where(c => c.UserId == userId).Select(c => new Authenticator() { Name = c.Name, Registered = c.RegDate, Id = c.Id }).ToListAsync();
        }

        [HttpDelete("{credId}")]
        public async Task<ActionResult> Delete(int credId)
        {
            var credential = await Find(credId);
            if (credential is null)
            {
                return NotFound();
            }
            _context.Remove(credential);
            await _context.SaveChangesAsync();
            return Ok();
        }

        private async Task<UserCredential?> Find(int credId)
        {
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            return await _context.UserCredentials.Where(c => c.UserId == userId && c.Id == credId).FirstOrDefaultAsync();
        }
    }
}
