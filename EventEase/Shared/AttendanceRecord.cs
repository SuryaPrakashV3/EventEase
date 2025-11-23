using System;

namespace EventBase.Shared
{
    public class AttendanceRecord
    {
        public string EventId { get; set; } = string.Empty;
        public string UserEmail { get; set; } = string.Empty;
        public string UserFullName { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
