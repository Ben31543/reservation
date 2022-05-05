using Microsoft.AspNetCore.Mvc;

namespace Reservation.Web.ApplicationUser
{
    public class ApplicationUser : Controller
    {
        private static object _locker = new object();

        private static long? memberId;
        protected static long? CurrentMemberId
        {
            get => memberId;
            set
            {
                lock (_locker)
                {
                    memberId = value;
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