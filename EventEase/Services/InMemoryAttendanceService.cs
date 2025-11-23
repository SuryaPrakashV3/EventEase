using EventBase.Shared;

namespace EventBase.Services
{
    public class InMemoryAttendanceService : IAttendanceService
    {
        private readonly List<AttendanceRecord> _store = new();

        public Task MarkAttendanceAsync(string eventId, AttendanceRecord record)
        {
            // Avoid duplicate marks for same user/event
            if (!_store.Any(r => r.EventId == eventId && r.UserEmail == record.UserEmail))
            {
                _store.Add(record);
            }
            return Task.CompletedTask;
        }

        public Task<List<AttendanceRecord>> GetAttendanceForEventAsync(string eventId)
        {
            var list = _store.Where(r => r.EventId == eventId).ToList();
            return Task.FromResult(list);
        }

        public Task<bool> HasUserMarkedAsync(string eventId, string userEmail)
        {
            var exists = _store.Any(r => r.EventId == eventId && r.UserEmail == userEmail);
            return Task.FromResult(exists);
        }
    }
}
