using EventBase.Shared;

namespace EventBase.Services
{
    public interface IAttendanceService
    {
        Task MarkAttendanceAsync(string eventId, AttendanceRecord record);
        Task<List<AttendanceRecord>> GetAttendanceForEventAsync(string eventId);
        Task<bool> HasUserMarkedAsync(string eventId, string userEmail);
    }
}
