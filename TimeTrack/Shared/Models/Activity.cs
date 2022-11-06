using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeTrack.Shared.Models
{
    public class Activity
    {
        public int Id { get; set; }
        [Column(TypeName = "date")]
        public DateTime Start { get; set; }
        [Comment("Duration in minutes")]
        public int Duration { get; set; }
        public int ClientId { get; set; }
        public Client? Client { get; set; }

        public List<Assessment>? Assessments { get; set; }
    }
}
