using System.Text.Json;

namespace EventBase.Services
{
    // Simple session state with change notification and lightweight persistence helper methods.
    public class UserSessionService
    {
        public bool IsLoggedIn { get; private set; }
        public string UserFullName { get; private set; } = string.Empty;
        public string UserEmail { get; private set; } = string.Empty;

        // Event triggered when session changes. Components can subscribe to re-render.
        public event Action? OnChange;

        public void SetUser(string fullName, string email)
        {
            UserFullName = fullName;
            UserEmail = email;
            IsLoggedIn = true;
            OnChange?.Invoke();
        }

        public void Clear()
        {
            UserFullName = string.Empty;
            UserEmail = string.Empty;
            IsLoggedIn = false;
            OnChange?.Invoke();
        }

        // Return a JSON representation useful for persistence in browser storage.
        public string ToJson()
        {
            var dto = new { IsLoggedIn, UserFullName, UserEmail };
            return JsonSerializer.Serialize(dto);
        }

        // Restore from a JSON string produced by ToJson. Returns whether restore succeeded.
        public bool RestoreFromJson(string? json)
        {
            if (string.IsNullOrWhiteSpace(json))
                return false;

            try
            {
                var doc = JsonSerializer.Deserialize<SessionDto>(json);
                if (doc is null) return false;
                UserFullName = doc.UserFullName ?? string.Empty;
                UserEmail = doc.UserEmail ?? string.Empty;
                IsLoggedIn = doc.IsLoggedIn;
                OnChange?.Invoke();
                return true;
            }
            catch
            {
                return false;
            }
        }

        private record SessionDto(bool IsLoggedIn, string? UserFullName, string? UserEmail);
    }
}
