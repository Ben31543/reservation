using System;
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
                if (value == null) throw new ArgumentNullException(nameof(value));
                lock (_locker)
                {
                    _serviceMemberId = value;
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
        protected void Authenticate(string userName, long serviceMemberId)
        {
            lock (_locker)
            {
                Username = userName;
                IsAuthorized = true;
                CurrentServiceMemberId = serviceMemberId;
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