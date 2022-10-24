using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TimeTrack.Server.Data;
using TimeTrack.Shared.Models;
using Microsoft.EntityFrameworkCore;
using VM = TimeTrack.Shared.ViewModels;

namespace TimeTrack.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly TimeContext _context;

        public CategoriesController(TimeContext timeContext)
        {
            this._context = timeContext;
        }

        [HttpGet]
        [ResponseCache(Duration = 7200)]
        public async Task<ICollection<VM.Category>> GetCategories()
        {
            return await _context.Categories.OrderBy(c => c.Name).Select(c => new VM.Category(c.Name) { Id = c.Id, Type = c.Type }).ToListAsync();
        }
    }
}
