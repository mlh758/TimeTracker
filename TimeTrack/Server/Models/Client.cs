﻿using System.ComponentModel.DataAnnotations;

namespace TimeTrack.Server.Models
{
    /*
     * To the user, there isn't a practical difference between categories and custom
     * categories. Instead of making them select Other and then one of their custom values
     * the application combines them in drop downs and works it out on save.
     */
    public class Client
    {
        public long Id { get; set; }
        [MaxLength(16)]
        public string Abbreviation { get; set; }

        public long AgeId { get; set; }
        public Category? Age { get; set; }
        public long SettingId { get; set; }
        public Category? Setting { get; set; }
        public long SexualOrientationId { get; set; }
        public Category? SexualOrientation { get; set; }
        public long RaceId { get; set; }
        public Category? Race { get; set; }
        public long GenderId { get; set; }
        public Category? Gender { get; set; }
        public List<Category>? Disabilities { get; set; }

        public long? CustomAgeId { get; set; }
        public CustomCategory? CustomAge { get; set; }
        public long? CustomSettingId { get; set; }
        public CustomCategory? CustomSetting { get; set; }
        public long? CustomSexualOrientationId { get; set; }
        public CustomCategory? CustomSexualOrientation { get; set; }
        public long? CustomRaceId { get; set; }
        public CustomCategory? CustomRace { get; set; }
        public long? CustomGenderId { get; set; }
        public CustomCategory? CustomGender { get; set; }
        public List<CustomCategory>? CustomDisabilities { get; set; }
        public List<ClientCustomDisability>? ClientCustomDisabilities { get; set; }

        public string UserId { get; set; }
        public User? User { get; set; }

        public IEnumerable<Group> Groups { get; set; }

        public List<Activity>? Activities { get; set; }

        public Client(string abbreviation)
        {
            Abbreviation = abbreviation;
        }
    }
}
