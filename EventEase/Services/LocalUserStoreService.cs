using EventBase.Shared;
using Microsoft.JSInterop;
using System.Text.Json;

namespace EventBase.Services
{
    public class LocalUserStoreService : IUserStoreService
    {
        private const string UsersKey = "eventease.users";
        private readonly IJSRuntime _js;

        public LocalUserStoreService(IJSRuntime js)
        {
            _js = js;
        }

        public async Task<bool> AddUserAsync(RegisteredUser user)
        {
            var users = await GetAllUsersAsync();
            if (users.Any(u => string.Equals(u.Email, user.Email, System.StringComparison.OrdinalIgnoreCase)))
                return false;
            users.Add(user);
            var json = JsonSerializer.Serialize(users);
            await _js.InvokeVoidAsync("EventEaseSession.set", UsersKey, json);
            return true;
        }

        public async Task<List<RegisteredUser>> GetAllUsersAsync()
        {
            try
            {
                var json = await _js.InvokeAsync<string?>("EventEaseSession.get", UsersKey);
                if (string.IsNullOrWhiteSpace(json)) return new List<RegisteredUser>();
                return JsonSerializer.Deserialize<List<RegisteredUser>>(json) ?? new List<RegisteredUser>();
            }
            catch
            {
                return new List<RegisteredUser>();
            }
        }

        public async Task<RegisteredUser?> GetUserByEmailAsync(string email)
        {
            var users = await GetAllUsersAsync();
            return users.FirstOrDefault(u => string.Equals(u.Email, email, System.StringComparison.OrdinalIgnoreCase));
        }
    }
}
