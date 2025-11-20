using System;
using System.Threading.Tasks;

namespace EventBase.Services
{
    public class UserSessionService
    {
        public bool IsLoggedIn { get; private set; }
        public string UserFullName { get; private set; } = string.Empty;
        public string UserEmail { get; private set; } = string.Empty;

        public void SetUser(string fullName, string email)
        {
            UserFullName = fullName;
            UserEmail = email;
            IsLoggedIn = true;
        }

        public void Clear()
        {
            UserFullName = string.Empty;
            UserEmail = string.Empty;
            IsLoggedIn = false;
        }
    }
}
