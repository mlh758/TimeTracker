using TimeTrack.Server.Models;
using TimeTrack.Server.Data;
using Microsoft.EntityFrameworkCore;

namespace TimeTrack.Server.Repositories
{
    public interface IAssessmentRepository
    {
        public Task<IEnumerable<Assessment>> FindById(IEnumerable<int> ids);
    }
    public class AssessmentRepository : IAssessmentRepository
    {
        private readonly TimeContext _context;
        public AssessmentRepository(TimeContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Assessment>> FindById(IEnumerable<int> ids)
        {
            return await _context.Assessments.Where(a => ids.Contains(a.Id)).ToListAsync();
        }
    }
}
