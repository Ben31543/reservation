using Microsoft.AspNetCore.Mvc;

namespace Reservation.Web.ApplicationUser
{
    public class ApplicationUser : Controller
    {
        private static object _locker = new object();

        private static long? _memberId;
        protected static long? CurrentMemberId
        {
            get => _memberId;
            set
            {
                lock (_locker)
                {
                    _memberId = value;
                }
            }
        }

        private static string _username;
        protected static string Username
        {
            get => _username;
            set
            {
                lock (_locker)
                {
                    _username = value;
                }
            }
        }

        private static bool _isAuthorized;
        protected static bool IsAuthorized
        {
            get => _isAuthorized;
            set
            {
                lock (_locker)
                {
                    _isAuthorized = value;
                }
            }
        }

        [NonAction]
        protected void Authenticate(string userName, long memberId)
        {
            lock (_locker)
            {
                CurrentMemberId = memberId;
                Username = userName;
                IsAuthorized = true;
            }
        }

        [NonAction]
        protected void LogoutUser()
        {
            lock (_locker)
            {
                Username = null;
                CurrentMemberId = null;
                IsAuthorized = false;
            }
        }
    }
}