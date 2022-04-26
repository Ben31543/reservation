using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Reservation.Data.Entities;
using Reservation.Resources.Constants;

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
                string hashedPassword = BitConverter.ToString(hashBytes).Replace("-", string.Empty);

                return hashedPassword;
            }
        }

        public static string GetErrorMessages(this ModelStateDictionary model)
        {
            StringBuilder result = new StringBuilder();
            IEnumerable<string> errorMessages = model.Values
                                           .SelectMany(v => v.Errors)
                                           .Select(i => i.ErrorMessage);

            result.AppendJoin(", ", errorMessages);
            return result.ToString();
        }

        public static bool IsValidArmPhoneNumber(this string phoneNumber)
        {
            if (phoneNumber != null)
            {
                if (phoneNumber.StartsWith("0"))
                {
                    phoneNumber = phoneNumber.Substring(1);
                }
                else if (phoneNumber.StartsWith("374"))
                {
                    phoneNumber = phoneNumber.Substring(3);
                }
                else if (phoneNumber.StartsWith("+374"))
                {
                    phoneNumber = phoneNumber.Substring(4);
                }

                if (phoneNumber.Length == 8 && CommonConstants.ArmenianPhoneOperators.Contains(phoneNumber.Substring(0, 2)))
                {
                    return true;
                }
            }

            return false;
        }

        public static bool IsValidEmail(this string email)
		{
            email = email.Trim();

            if (email.EndsWith("."))
            {
                return false;
            }

            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
        
        public static bool IsBase64(this string base64String)
        {
            if (string.IsNullOrEmpty(base64String))
            {
                return false;
            }

            if (base64String.Contains("data:image/") && base64String.Length > 500)
            {
                return true;
            }

            if (base64String.Length % 4 != 0 ||
                base64String.Contains(" ") ||
                base64String.Contains("\t") ||
                base64String.Contains("\r") ||
                base64String.Contains("\n"))
            {
                return false;
            }

            try
            {
                Convert.FromBase64String(base64String);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static string ToDisplayFormat(this BankAccount bankAccount)
        {
            if (bankAccount == null)
            {
                return "-";
            }

            return $"{bankAccount.AccountNumber}\n{bankAccount.Owner}\n{bankAccount.Bank.Name}";
        }
    }
}
