using System.ComponentModel.DataAnnotations;

namespace TimeTrack.Shared.Models
{
    public class Client
    {
        public int Id { get; set; }
        public string Abbreviation { get; set; }

        [DataType(DataType.Date)]
        [Required]
        public DateTime DateOfBirth { get; set; }

        public int UserId { get; set; }
        public User? User { get; set; }

        public List<Activity>? Activities { get; set; }

        public Client(string abbreviation)
        {
            Abbreviation = abbreviation;
        }
    }
}
