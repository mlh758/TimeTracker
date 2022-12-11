using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TimeTrack.Server.Repositories;
using TimeTrack.Shared;
using VM = TimeTrack.Shared.ViewModels;
using TimeTrack.Server.Models;

namespace TimeTrack.Server.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    [Authorize]
    public class GroupsController : Controller
    {
        private readonly IGroupRepository _groupRepository;

        public GroupsController(IGroupRepository groupRepository)
        {
            _groupRepository = groupRepository;
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<VM.Group>> GetGroup(int id)
        {
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var group = await _groupRepository.Find(userId, id);

            if (group is null)
            {
                return NotFound();
            }

            return new VM.Group()
            {
                Name = group.Name,
                Clients = group.Clients!.Select(c => new VM.ActivityOwner() { Id = c.Id, Name = c.Abbreviation }).ToArray(),
            };
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<VM.ActivityOwner>>> GetGroups()
        {
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var groups = await _groupRepository.All(userId);
            return groups.Select(group => MapToView(group)).ToList();
        }

        [HttpDelete("{id}")]
        public async Task Destroy(long id)
        {
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _groupRepository.Destroy(userId, id);
        }

        [HttpPost]
        public async Task<VM.ActivityOwner> Create(GroupForm form)
        {
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var group = await _groupRepository.Create(userId, form);
            return MapToView(group);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<VM.Group>> Update(long id, GroupForm form)
        {
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var group = await _groupRepository.Update(userId, id, form);

            if (group is null)
            {
                return NotFound();
            }

            return new VM.Group()
            {
                Name = group.Name,
                Clients = group.Clients!.Select(c => new VM.ActivityOwner() { Id = c.Id, Name = c.Abbreviation }).ToArray(),
            };

        }

        private VM.ActivityOwner MapToView(Group group)
        {
            return new VM.ActivityOwner() { Id = group.Id, Name = group.Name };
        }
    }
}
