using System.Collections.Generic;
using System.Threading.Tasks;
using EventBase.Shared;

namespace EventBase.Services
{
    public interface IEventService
    {
        Task<List<EventItem>> GetEventsAsync();
        Task<EventItem?> GetEventByIdAsync(string id);
    }
}
