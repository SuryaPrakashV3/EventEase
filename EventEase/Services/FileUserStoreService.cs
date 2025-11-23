using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using EventBase.Shared;

namespace EventBase.Services
{
    // File-backed user store. Persists a JSON array of RegisteredUser to disk.
    public class FileUserStoreService : IUserStoreService
    {
        private readonly string _filePath;
        private readonly SemaphoreSlim _sema = new(1, 1);

        public FileUserStoreService(IHostEnvironment env)
        {
            // store under {contentRoot}/Data/users.json
            var dataDir = Path.Combine(env.ContentRootPath ?? Directory.GetCurrentDirectory(), "Data");
            if (!Directory.Exists(dataDir)) Directory.CreateDirectory(dataDir);
            _filePath = Path.Combine(dataDir, "users.json");
        }

        public async Task<bool> AddUserAsync(RegisteredUser user)
        {
            await _sema.WaitAsync();
            try
            {
                var users = await ReadAllAsync();
                if (users.Any(u => string.Equals(u.Email, user.Email, System.StringComparison.OrdinalIgnoreCase)))
                    return false;
                users.Add(user);
                await WriteAllAsync(users);
                return true;
            }
            finally
            {
                _sema.Release();
            }
        }

        public async Task<List<RegisteredUser>> GetAllUsersAsync()
        {
            await _sema.WaitAsync();
            try
            {
                return await ReadAllAsync();
            }
            finally
            {
                _sema.Release();
            }
        }

        public async Task<RegisteredUser?> GetUserByEmailAsync(string email)
        {
            await _sema.WaitAsync();
            try
            {
                var users = await ReadAllAsync();
                return users.FirstOrDefault(u => string.Equals(u.Email, email, System.StringComparison.OrdinalIgnoreCase));
            }
            finally
            {
                _sema.Release();
            }
        }

        private async Task<List<RegisteredUser>> ReadAllAsync()
        {
            if (!File.Exists(_filePath)) return new List<RegisteredUser>();
            try
            {
                using var fs = File.OpenRead(_filePath);
                return await JsonSerializer.DeserializeAsync<List<RegisteredUser>>(fs) ?? new List<RegisteredUser>();
            }
            catch
            {
                return new List<RegisteredUser>();
            }
        }

        private async Task WriteAllAsync(List<RegisteredUser> users)
        {
            // write to temp file then replace to reduce risk of corruption
            var tmp = _filePath + ".tmp";
            using (var fs = File.Create(tmp))
            {
                await JsonSerializer.SerializeAsync(fs, users, new JsonSerializerOptions { WriteIndented = true });
            }
            File.Copy(tmp, _filePath, overwrite: true);
            File.Delete(tmp);
        }
    }
}
