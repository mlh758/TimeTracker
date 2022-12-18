using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTrack.Shared.Enums
{
    public enum CategoryType
    {
        Age,
        Gender,
        TreatmentSetting,
        Disability,
        Race,
        SexualOrientation
    }

    public static class CategoryDisplay
    {
        public static string Show(CategoryType type)
        {
            return type switch
            {
                CategoryType.Age => "Age",
                CategoryType.Gender => "Gender",
                CategoryType.TreatmentSetting => "Treatment Setting",
                CategoryType.Disability => "Disability",
                CategoryType.Race => "Race",
                CategoryType.SexualOrientation => "Sexual Orientation",
                _ => throw new ArgumentOutOfRangeException(nameof(type)),
            };
        }
    }
}
