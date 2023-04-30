using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TimeTrack.Server.Data;
using VM = TimeTrack.Shared.ViewModels;

namespace TimeTrack.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ActivityGroupingController : ControllerBase
    {
        private readonly TimeContext _context;
        public ActivityGroupingController(TimeContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ICollection<VM.ActivityGrouping>> GetGroupings()
        {
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            return await _context.ActivityGrouping.Where(g => g.UserId == userId || g.UserId == null).OrderBy(c => c.Name).Select(c => new VM.ActivityGrouping(c.Name) { Id = c.Id, Type = c.GroupingType }).ToListAsync();
        }
    }
}
