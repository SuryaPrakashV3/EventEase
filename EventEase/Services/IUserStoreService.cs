using System.Threading.Tasks;
using EventBase.Shared;
using System.Collections.Generic;

namespace EventBase.Services
{
    public interface IUserStoreService
    {
        Task<bool> AddUserAsync(RegisteredUser user);
        Task<RegisteredUser?> GetUserByEmailAsync(string email);
        Task<List<RegisteredUser>> GetAllUsersAsync();
    }
}
