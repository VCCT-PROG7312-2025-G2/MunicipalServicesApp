using MunicipalServicesApp.Models;
using MunicipalServicesApp.Models.CustomCollections;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MunicipalServicesApp.Services
{
    public class EventService
    {
        private CustomDictionary<string, Event> _events;
        private SortedDictionary<DateTime, EventSet<string>> _eventsByDate;
        private CustomDictionary<string, EventSet<string>> _eventsByCategory;
        private EventPriorityQueue _upcomingEvents;
        private Queue<UserSearch> _recentSearches;
        private EventSet<string> _uniqueCategories;
        private EventSet<DateTime> _uniqueDates;

        public EventService()
        {
            _events = new CustomDictionary<string, Event>();
            _eventsByDate = new SortedDictionary<DateTime, EventSet<string>>();
            _eventsByCategory = new CustomDictionary<string, EventSet<string>>();
            _upcomingEvents = new EventPriorityQueue(true);
            _recentSearches = new Queue<UserSearch>();
            _uniqueCategories = new EventSet<string>();
            _uniqueDates = new EventSet<DateTime>();

            InitializeSampleData();
        }

        private void InitializeSampleData()
        {
            var sampleEvents = new[]
            {
                new Event {
                    Title = "Community Clean-up Day",
                    Description = "Join your neighbors for a community-wide clean-up event. We'll provide gloves and bags! Help make our city cleaner and greener.",
                    Category = "Environmental",
                    EventDate = DateTime.Now.AddDays(3),
                    Location = "Central Park",
                    Organizer = "City Council",
                    Capacity = 100,
                    CurrentAttendees = 45,
                    IsFeatured = true
                },
                new Event {
                    Title = "Summer Music Festival",
                    Description = "Annual summer music festival featuring local bands and food vendors. Three stages with continuous entertainment from noon to midnight.",
                    Category = "Arts & Music",
                    EventDate = DateTime.Now.AddDays(7),
                    Location = "Riverside Amphitheater",
                    Organizer = "Arts Commission",
                    Capacity = 500,
                    CurrentAttendees = 320,
                    IsFeatured = true
                },
                new Event {
                    Title = "Budget Planning Meeting",
                    Description = "Public meeting to discuss the upcoming municipal budget. All residents are encouraged to attend and provide input.",
                    Category = "Community Meeting",
                    EventDate = DateTime.Now.AddDays(1),
                    Location = "City Hall",
                    Organizer = "Finance Department",
                    Capacity = 200,
                    CurrentAttendees = 78,
                    IsFeatured = false
                },
                new Event {
                    Title = "Youth Soccer Tournament",
                    Description = "Annual youth soccer tournament for ages 8-16. Registration required. Trophies for winning teams in each age group.",
                    Category = "Sports",
                    EventDate = DateTime.Now.AddDays(5),
                    Location = "Community Sports Complex",
                    Organizer = "Parks & Recreation",
                    Capacity = 300,
                    CurrentAttendees = 210,
                    IsFeatured = false
                },
                new Event {
                    Title = "Emergency Preparedness Workshop",
                    Description = "Learn how to prepare for emergencies and natural disasters. Free emergency kits for the first 50 attendees.",
                    Category = "Emergency",
                    EventDate = DateTime.Now.AddDays(2),
                    Location = "Public Safety Building",
                    Organizer = "Emergency Management",
                    Capacity = 150,
                    CurrentAttendees = 92,
                    IsFeatured = true
                },
                new Event {
                    Title = "Farmers Market Opening Day",
                    Description = "Season opening of the downtown farmers market featuring local produce, crafts, and live cooking demonstrations.",
                    Category = "Festival",
                    EventDate = DateTime.Now.AddDays(4),
                    Location = "Main Street Plaza",
                    Organizer = "Downtown Association",
                    Capacity = 400,
                    CurrentAttendees = 185,
                    IsFeatured = true
                },
                new Event {
                    Title = "Library Book Sale",
                    Description = "Annual library book sale with thousands of books at discounted prices. All proceeds support library programs.",
                    Category = "Cultural",
                    EventDate = DateTime.Now.AddDays(6),
                    Location = "Public Library",
                    Organizer = "Friends of the Library",
                    Capacity = 100,
                    CurrentAttendees = 67,
                    IsFeatured = false
                },
                new Event {
                    Title = "Tree Planting Initiative",
                    Description = "Help plant 100 new trees in our urban areas. No experience necessary - training and tools provided.",
                    Category = "Environmental",
                    EventDate = DateTime.Now.AddDays(8),
                    Location = "Various Locations",
                    Organizer = "Urban Forestry Division",
                    Capacity = 80,
                    CurrentAttendees = 42,
                    IsFeatured = true
                },
                new Event {
                    Title = "Senior Technology Workshop",
                    Description = "Learn how to use smartphones, tablets, and social media. One-on-one assistance available.",
                    Category = "Senior",
                    EventDate = DateTime.Now.AddDays(10),
                    Location = "Senior Center",
                    Organizer = "Community Services",
                    Capacity = 50,
                    CurrentAttendees = 38,
                    IsFeatured = false
                },
                new Event {
                    Title = "Neighborhood Watch Meeting",
                    Description = "Quarterly neighborhood watch meeting with police department representatives. Discuss community safety concerns.",
                    Category = "Community Meeting",
                    EventDate = DateTime.Now.AddDays(12),
                    Location = "Community Center",
                    Organizer = "Police Department",
                    Capacity = 120,
                    CurrentAttendees = 89,
                    IsFeatured = false
                },
                new Event {
                    Title = "Annual Charity Run",
                    Description = "5K and 10K races to support local food bank. Registration includes t-shirt and post-race refreshments.",
                    Category = "Sports",
                    EventDate = DateTime.Now.AddDays(14),
                    Location = "Riverside Park",
                    Organizer = "Parks & Recreation",
                    Capacity = 1000,
                    CurrentAttendees = 650,
                    IsFeatured = true
                },
                new Event {
                    Title = "Art in the Park Exhibition",
                    Description = "Local artists display and sell their work in an outdoor gallery setting. Live music and artist demonstrations.",
                    Category = "Arts & Music",
                    EventDate = DateTime.Now.AddDays(9),
                    Location = "Memorial Park",
                    Organizer = "Arts Commission",
                    Capacity = 300,
                    CurrentAttendees = 156,
                    IsFeatured = true
                },
                new Event {
                    Title = "Small Business Workshop",
                    Description = "Learn about starting and growing a small business. Topics include licensing, financing, and marketing.",
                    Category = "Business",
                    EventDate = DateTime.Now.AddDays(11),
                    Location = "Business Development Center",
                    Organizer = "Economic Development",
                    Capacity = 75,
                    CurrentAttendees = 48,
                    IsFeatured = false
                },
                new Event {
                    Title = "Holiday Parade Planning",
                    Description = "Volunteer meeting to plan the annual holiday parade. All interested community members welcome.",
                    Category = "Community Meeting",
                    EventDate = DateTime.Now.AddDays(15),
                    Location = "City Hall Annex",
                    Organizer = "Events Committee",
                    Capacity = 60,
                    CurrentAttendees = 32,
                    IsFeatured = false
                },
                new Event {
                    Title = "Recycling Awareness Day",
                    Description = "Learn about proper recycling practices and new recycling technologies. Bring your electronic waste for safe disposal.",
                    Category = "Environmental",
                    EventDate = DateTime.Now.AddDays(13),
                    Location = "Recycling Center",
                    Organizer = "Public Works",
                    Capacity = 200,
                    CurrentAttendees = 123,
                    IsFeatured = true
                },
                new Event {
                    Title = "Youth Basketball League Finals",
                    Description = "Championship games for all youth basketball divisions. Concession stands open. Free admission.",
                    Category = "Youth",
                    EventDate = DateTime.Now.AddDays(16),
                    Location = "Community Gymnasium",
                    Organizer = "Parks & Recreation",
                    Capacity = 500,
                    CurrentAttendees = 420,
                    IsFeatured = false
                },
                new Event {
                    Title = "Historical Society Lecture",
                    Description = "Monthly lecture series: 'The History of Our Downtown Architecture'. Light refreshments served.",
                    Category = "Cultural",
                    EventDate = DateTime.Now.AddDays(18),
                    Location = "Historical Museum",
                    Organizer = "Historical Society",
                    Capacity = 100,
                    CurrentAttendees = 72,
                    IsFeatured = false
                },
                new Event {
                    Title = "Community Garden Harvest Festival",
                    Description = "Celebrate the harvest from our community gardens. Food tasting, gardening workshops, and family activities.",
                    Category = "Festival",
                    EventDate = DateTime.Now.AddDays(20),
                    Location = "Community Gardens",
                    Organizer = "Urban Agriculture Program",
                    Capacity = 250,
                    CurrentAttendees = 168,
                    IsFeatured = true
                },
                new Event {
                    Title = "Jazz in the Evening",
                    Description = "Outdoor jazz concert featuring local musicians. Bring blankets and chairs. Food trucks available.",
                    Category = "Arts & Music",
                    EventDate = DateTime.Now.AddDays(17),
                    Location = "Sunset Park",
                    Organizer = "Arts Commission",
                    Capacity = 400,
                    CurrentAttendees = 289,
                    IsFeatured = true
                },
                new Event {
                    Title = "Public Transit Forum",
                    Description = "Discuss upcoming changes to bus routes and schedules. Provide feedback to transit planners.",
                    Category = "Community Meeting",
                    EventDate = DateTime.Now.AddDays(19),
                    Location = "Transit Center",
                    Organizer = "Transit Authority",
                    Capacity = 150,
                    CurrentAttendees = 94,
                    IsFeatured = false
                },
                new Event {
                    Title = "Coastal Cleanup Initiative",
                    Description = "Help clean up our local beaches and coastal areas. All equipment provided. Lunch included for volunteers.",
                    Category = "Environmental",
                    EventDate = DateTime.Now.AddDays(22),
                    Location = "Ocean Beach Park",
                    Organizer = "Environmental Protection",
                    Capacity = 120,
                    CurrentAttendees = 78,
                    IsFeatured = true
                },
                new Event {
                    Title = "Tech Innovation Fair",
                    Description = "Showcase of local tech startups and innovation projects. Networking opportunities and guest speakers.",
                    Category = "Business",
                    EventDate = DateTime.Now.AddDays(25),
                    Location = "Convention Center",
                    Organizer = "Technology Council",
                    Capacity = 800,
                    CurrentAttendees = 520,
                    IsFeatured = true
                },
                new Event {
                    Title = "Senior Health Fair",
                    Description = "Free health screenings, wellness information, and fitness demonstrations for seniors.",
                    Category = "Health & Wellness",
                    EventDate = DateTime.Now.AddDays(21),
                    Location = "Senior Center",
                    Organizer = "Health Department",
                    Capacity = 200,
                    CurrentAttendees = 145,
                    IsFeatured = false
                },
                new Event {
                    Title = "Youth Coding Camp",
                    Description = "Week-long coding camp for teenagers interested in computer programming. No prior experience required.",
                    Category = "Youth",
                    EventDate = DateTime.Now.AddDays(24),
                    Location = "Technology Center",
                    Organizer = "Education Department",
                    Capacity = 60,
                    CurrentAttendees = 45,
                    IsFeatured = true
                },
                new Event {
                    Title = "Cultural Diversity Celebration",
                    Description = "Celebrate our city's cultural diversity with performances, food, and exhibits from around the world.",
                    Category = "Cultural",
                    EventDate = DateTime.Now.AddDays(28),
                    Location = "City Park",
                    Organizer = "Cultural Affairs",
                    Capacity = 1000,
                    CurrentAttendees = 780,
                    IsFeatured = true
                },
                new Event {
                    Title = "Emergency Response Drill",
                    Description = "City-wide emergency response drill involving multiple agencies. Volunteers needed to act as victims.",
                    Category = "Emergency",
                    EventDate = DateTime.Now.AddDays(26),
                    Location = "Emergency Operations Center",
                    Organizer = "Emergency Management",
                    Capacity = 200,
                    CurrentAttendees = 156,
                    IsFeatured = false
                },
                new Event {
                    Title = "Yoga in the Park",
                    Description = "Free community yoga sessions for all skill levels. Bring your own mat and water bottle.",
                    Category = "Health & Wellness",
                    EventDate = DateTime.Now.AddDays(23),
                    Location = "Sunrise Park",
                    Organizer = "Parks & Recreation",
                    Capacity = 100,
                    CurrentAttendees = 82,
                    IsFeatured = false
                },
                new Event {
                    Title = "Business Networking Breakfast",
                    Description = "Monthly networking event for local business professionals. Featured speaker on economic trends.",
                    Category = "Business",
                    EventDate = DateTime.Now.AddDays(27),
                    Location = "Chamber of Commerce",
                    Organizer = "Chamber of Commerce",
                    Capacity = 120,
                    CurrentAttendees = 95,
                    IsFeatured = false
                },
                new Event {
                    Title = "Winter Festival",
                    Description = "Annual winter festival with ice skating, holiday market, and tree lighting ceremony.",
                    Category = "Festival",
                    EventDate = DateTime.Now.AddDays(30),
                    Location = "Downtown Square",
                    Organizer = "Events Committee",
                    Capacity = 2000,
                    CurrentAttendees = 1250,
                    IsFeatured = true
                }
            };

            foreach (var eventItem in sampleEvents)
            {
                AddEvent(eventItem);
            }
        }

        public void AddEvent(Event eventItem)
        {
            _events.Add(eventItem.Id, eventItem);
            _upcomingEvents.Enqueue(eventItem);

            if (!_eventsByDate.ContainsKey(eventItem.EventDate.Date))
            {
                _eventsByDate[eventItem.EventDate.Date] = new EventSet<string>();
            }
            _eventsByDate[eventItem.EventDate.Date].Add(eventItem.Id);

            if (!_eventsByCategory.ContainsKey(eventItem.Category))
            {
                _eventsByCategory[eventItem.Category] = new EventSet<string>();
            }
            _eventsByCategory[eventItem.Category].Add(eventItem.Id);

            _uniqueCategories.Add(eventItem.Category);
            _uniqueDates.Add(eventItem.EventDate.Date);
        }

        public List<Event> SearchEvents(string searchTerm, string category, DateTime? date)
        {
            var search = new UserSearch
            {
                SearchTerm = searchTerm,
                Category = category,
                SearchDate = DateTime.Now
            };
            _recentSearches.Enqueue(search);
            if (_recentSearches.Count > 10)
            {
                _recentSearches.Dequeue();
            }

            var results = new EventSet<string>();

            // Search by text
            if (!string.IsNullOrEmpty(searchTerm))
            {
                foreach (var eventPair in _events)
                {
                    var eventItem = eventPair.Value;
                    if (eventItem.Title.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                        eventItem.Description.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                    {
                        results.Add(eventItem.Id);
                    }
                }
            }

            // Filter by category
            if (!string.IsNullOrEmpty(category) && _eventsByCategory.ContainsKey(category))
            {
                var categoryEvents = _eventsByCategory[category];
                if (results.Count == 0)
                {
                    // If no previous results, use all category events
                    results = categoryEvents;
                }
                else
                {
                    // Intersect with existing results
                    results = results.Intersection(categoryEvents);
                }
            }

            // Filter by date
            if (date.HasValue && _eventsByDate.ContainsKey(date.Value.Date))
            {
                var dateEvents = _eventsByDate[date.Value.Date];
                if (results.Count == 0)
                {
                    results = dateEvents;
                }
                else
                {
                    results = results.Intersection(dateEvents);
                }
            }

            // If no filters applied, return all events
            if (results.Count == 0 && string.IsNullOrEmpty(searchTerm) && string.IsNullOrEmpty(category) && !date.HasValue)
            {
                foreach (var eventPair in _events)
                {
                    results.Add(eventPair.Key);
                }
            }

            // Convert event IDs to Event objects
            var eventList = new List<Event>();
            foreach (var eventId in results)
            {
                if (_events.TryGetValue(eventId, out var eventItem))
                {
                    eventList.Add(eventItem);
                }
            }

            return eventList.OrderBy(e => e.EventDate).ToList();
        }

        public List<Event> GetRecommendedEvents()
        {
            var recommendations = new EventSet<string>();
            var recentSearch = _recentSearches.LastOrDefault();

            if (recentSearch != null)
            {
                // Recommend events from the same category
                if (!string.IsNullOrEmpty(recentSearch.Category) && _eventsByCategory.ContainsKey(recentSearch.Category))
                {
                    var categoryEvents = _eventsByCategory[recentSearch.Category];
                    foreach (var eventId in categoryEvents)
                    {
                        recommendations.Add(eventId);
                    }
                }

                // Recommend featured events
                foreach (var eventPair in _events)
                {
                    var eventItem = eventPair.Value;
                    if (eventItem.IsFeatured && eventItem.EventDate > DateTime.Now)
                    {
                        recommendations.Add(eventItem.Id);
                    }
                }

                // Recommend events with similar titles
                if (!string.IsNullOrEmpty(recentSearch.SearchTerm))
                {
                    foreach (var eventPair in _events)
                    {
                        var eventItem = eventPair.Value;
                        if (eventItem.Title.Contains(recentSearch.SearchTerm, StringComparison.OrdinalIgnoreCase))
                        {
                            recommendations.Add(eventItem.Id);
                        }
                    }
                }
            }

            // If no recent searches, return featured upcoming events
            if (recommendations.Count == 0)
            {
                foreach (var eventPair in _events)
                {
                    var eventItem = eventPair.Value;
                    if (eventItem.IsFeatured && eventItem.EventDate > DateTime.Now)
                    {
                        recommendations.Add(eventItem.Id);
                    }
                }
            }

            // Convert to Event objects and return
            var eventList = new List<Event>();
            foreach (var eventId in recommendations)
            {
                if (_events.TryGetValue(eventId, out var eventItem))
                {
                    eventList.Add(eventItem);
                }
            }

            return eventList.OrderBy(e => e.EventDate).Take(5).ToList();
        }

        public List<string> GetCategories()
        {
            return _uniqueCategories.ToList();
        }

        public List<DateTime> GetEventDates()
        {
            return _uniqueDates.ToList().OrderBy(d => d).ToList();
        }

        public List<Event> GetUpcomingEvents(int count = 10)
        {
            var upcoming = new List<Event>();
            var tempQueue = new EventPriorityQueue(true);

            // Dequeue events until we get the requested count or queue is empty
            while (_upcomingEvents.Count > 0 && upcoming.Count < count)
            {
                var nextEvent = _upcomingEvents.Dequeue();
                if (nextEvent.EventDate >= DateTime.Now)
                {
                    upcoming.Add(nextEvent);
                }
                tempQueue.Enqueue(nextEvent);
            }

            // Restore the remaining events back to the main queue
            while (tempQueue.Count > 0)
            {
                _upcomingEvents.Enqueue(tempQueue.Dequeue());
            }

            return upcoming;
        }

        // Helper method to get all events (for debugging)
        public List<Event> GetAllEvents()
        {
            var events = new List<Event>();
            foreach (var eventPair in _events)
            {
                events.Add(eventPair.Value);
            }
            return events.OrderBy(e => e.EventDate).ToList();
        }
    }
}