using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventBase.Shared;

namespace EventBase.Services
{
    public class InMemoryEventService : IEventService
    {
        private readonly List<EventItem> seed = new()
        {
            new EventItem { Id = "1", Name = "Corporate Summit 2025", Date = DateTime.Today.AddDays(14), Location = "Grand Hall, City Center", ShortDescription = "A gathering of industry leaders to discuss strategy.", Description = "Full day summit with keynotes, panels, and networking." },
            new EventItem { Id = "2", Name = "Summer Gala", Date = DateTime.Today.AddMonths(1), Location = "Lakeside Pavilion", ShortDescription = "An evening of dining and entertainment.", Description = "Black-tie event with dinner, auction, and performances." },
            new EventItem { Id = "3", Name = "Community Workshop", Date = DateTime.Today.AddDays(7), Location = "Community Center", ShortDescription = "Hands-on workshops for skill-building.", Description = "Multiple breakout sessions and practical exercises." }
        };

        public Task<List<EventItem>> GetEventsAsync()
        {
            Task.Delay(2000);
            // In a real app this would be async I/O.
            return Task.FromResult(seed.ToList());
        }

        public Task<EventItem?> GetEventByIdAsync(string id)
        {
            var found = seed.FirstOrDefault(e => e.Id == id);
            return Task.FromResult(found);
        }
    }
}
