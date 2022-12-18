using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TimeTrack.Server.Data;
using TimeTrack.Server.Models;
using VM = TimeTrack.Shared.ViewModels;
using TimeTrack.Shared;

namespace TimeTrack.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    /*
     * Custom Categories are just categories that are linked to a specific user. Separated into their own controller
     * because normally we don't edit categories and it is easier to isolate the user specific logic here.
     */
    public class CustomCategoriesController : ControllerBase
    {
        private readonly TimeContext _context;

        public CustomCategoriesController(TimeContext timeContext)
        {
            this._context = timeContext;
        }

        [HttpGet]
        public async Task<ICollection<VM.Category>> GetCategories()
        {
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            return await _context.Categories.Where(c => c.UserId == userId).OrderBy(c => c.Name).Select(c => new VM.Category(c.Name) { Id = c.Id, Type = c.Type }).ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult> CreateCategory(CustomCategoryForm form)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _context.Users.FindAsync(userId);
            if (user is null)
            {
                return Forbid();
            }
            var newCategory = new Category(form.Name!, form.Type) { UserId = userId };
            _context.Categories.Add(newCategory);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCategory(long id)
        {
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var saved = await _context.Categories.Where(c => c.UserId == userId && c.Id == id && c.UserId != null).FirstOrDefaultAsync();
            if (saved is null)
            {
                return NotFound();
            }
            _context.Remove(saved);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
