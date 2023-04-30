using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TimeTrack.Server.Data;
using TimeTrack.Server.Models;
using Microsoft.EntityFrameworkCore;
using VM = TimeTrack.Shared.ViewModels;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace TimeTrack.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoriesController : ControllerBase
    {
        private readonly TimeContext _context;

        public CategoriesController(TimeContext timeContext)
        {
            this._context = timeContext;
        }

        [HttpGet]
        [ResponseCache(Duration = 120)]
        public async Task<ICollection<VM.Category>> GetCategories()
        {
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            return await _context.Categories.Where(c => c.UserId == null || c.UserId == userId).OrderBy(c => c.Name).Select(c => new VM.Category(c.Name) { Id = c.Id, Type = c.Type }).ToListAsync();
        }
    }
}
