using System.ComponentModel.DataAnnotations;
using TimeTrack.Shared.Enums;

namespace TimeTrack.Shared
{
    public class CustomCategoryForm
    {
        [Required]
        public string? Name { get; set; }
        [Required]
        public CategoryType Type { get; set; }
    }
}
