using Microsoft.AspNetCore.Mvc;

namespace Reservation.ServiceMember
{
    public class ApplicationUser : Controller
    {
        private static object _locker = new object();

        private static long? _serviceMemberId;
        protected static long? CurrentServiceMemberId
        {
            get => _serviceMemberId;
            set
            {
                lock (_locker)
                {
                    _serviceMemberId = value;
                }
            }
        }

        private static string username;
        protected static string Username
        {
            get => username;
            set
            {
                lock (_locker)
                {
                    username = value;
                }
            }
        }

        private static bool isAuthorized;
        protected static bool IsAuthorized
        {
            get => isAuthorized;
            set
            {
                lock (_locker)
                {
                    isAuthorized = value;
                }
            }
        }

        [NonAction]
        protected void Authenticate(string userName)
        {
            lock (_locker)
            {
                Username = userName;
                IsAuthorized = true;
            }
        }

        [NonAction]
        protected void LogoutServiceMember()
        {
            lock (_locker)
            {
                Username = null;
                CurrentServiceMemberId = null;
                IsAuthorized = false;
            }
        }
    }
}