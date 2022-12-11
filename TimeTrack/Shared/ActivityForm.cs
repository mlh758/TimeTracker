﻿using System.ComponentModel.DataAnnotations;
using TimeTrack.Shared.ViewModels;


namespace TimeTrack.Shared
{
    public class ActivityForm
    {
        [Required]
        public DateTime? Start { get; set; }
        [Required]
        [Range(0, 1440, ErrorMessage = "Duration is limited to the number of minutes in a day (0 - 1440)")] // 1440 is minutes in a 24 hour period
        public int? Duration { get; set; }

        public Schedule? Schedule { get; set; }

        [Required]
        public HashSet<Assessment> Assessments { get; set; }


        [CustomValidation(typeof(ActivityForm), "ClientOrGroupRequired", ErrorMessage = "You must select either a client or a group")]
        public ActivityOwner? Client { get; set; }

        [CustomValidation(typeof(ActivityForm), "ClientOrGroupRequired", ErrorMessage = "You must select either a client or a group")]
        public ActivityOwner? Group { get; set; }

        public ActivityForm()
        {
            Assessments = new HashSet<Assessment>();
        }

        public static bool ClientOrGroupRequired(ActivityForm form)
        {
            return form.Client is not null ^ form.Group is not null;
        }
    }
}
