using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Reservation.Service.Helpers
{
    public static class Extensions
    {
        public static string ToHashedPassword(this string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] sourceBytes = Encoding.UTF8.GetBytes(password);
                byte[] hashBytes = sha256Hash.ComputeHash(sourceBytes);
                string hashedPassword = BitConverter.ToString(hashBytes).Replace("-", String.Empty);

                return hashedPassword;
            }
        }
    }
}
