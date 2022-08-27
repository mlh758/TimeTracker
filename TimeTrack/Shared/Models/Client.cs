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

        public Client(string abbreviation)
        {
            Abbreviation = abbreviation;
        }
    }
}
