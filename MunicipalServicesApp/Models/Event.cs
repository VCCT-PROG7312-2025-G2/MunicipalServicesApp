using System;

namespace MunicipalServicesApp.Models
{
    public class Event
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public DateTime EventDate { get; set; }
        public string Location { get; set; }
        public string Organizer { get; set; }
        public int Capacity { get; set; }
        public int CurrentAttendees { get; set; }
        public bool IsFeatured { get; set; }
        public DateTime CreatedDate { get; set; }

        public Event()
        {
            Id = Guid.NewGuid().ToString();
            CreatedDate = DateTime.Now;
        }
    }

    public class EventCategory
    {
        public static readonly string[] Categories = {
            "Community Meeting", "Sports", "Cultural", "Educational",
            "Health & Wellness", "Environmental", "Arts & Music",
            "Business", "Youth", "Senior", "Emergency", "Festival"
        };
    }

    public class UserSearch
    {
        public string SearchTerm { get; set; }
        public string Category { get; set; }
        public DateTime SearchDate { get; set; }
        public int SearchCount { get; set; }

        public UserSearch()
        {
            SearchDate = DateTime.Now;
            SearchCount = 1;
        }
    }

    public class EventSearchViewModel
    {
        public string SearchTerm { get; set; }
        public string Category { get; set; }
        public DateTime? SearchDate { get; set; }
        public List<Event> Events { get; set; }
        public List<Event> Recommendations { get; set; }
        public List<string> Categories { get; set; }

        public EventSearchViewModel()
        {
            Events = new List<Event>();
            Recommendations = new List<Event>();
            Categories = new List<string>();
        }
    }
}