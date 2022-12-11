using System.ComponentModel.DataAnnotations;
using TimeTrack.Shared.ViewModels;

namespace TimeTrack.Shared
{
    public class GroupForm
    {
        [MinLength(1, ErrorMessage = "A group requires at least one client")]
        public List<ActivityOwner> Clients { get; set; } = new List<ActivityOwner>();
        [Required]
        public string? Name { get; set; }
    }
}
