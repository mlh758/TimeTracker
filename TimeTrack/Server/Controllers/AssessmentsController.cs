using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TimeTrack.Server.Data;
using TimeTrack.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace TimeTrack.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssessmentsController : ControllerBase
    {
        private readonly TimeContext _context;

        public AssessmentsController(TimeContext timeContext)
        {
            this._context = timeContext;
        }

        [HttpGet]
        [ResponseCache(Duration = 7200)]
        public async Task<ICollection<Assessment>> GetAssessments()
        {
            return await _context.Assessments.ToListAsync();
        }
    }
}
