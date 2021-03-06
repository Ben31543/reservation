using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using Reservation.Data.Entities;
using Reservation.Models.Common;
using Reservation.Resources;
using Reservation.Resources.Constants;
using SixLabors.ImageSharp.PixelFormats;

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
        
        public static string ToDisplayFormat(this BankCard bankCard)
        {
            if (bankCard == null)
            {
                return "-";
            }

            return $"{bankCard.Number}\n{bankCard.Owner}\n{bankCard.Bank.Name}";
        }

        public static string ToProductsDisplayFormat(this string productsJson)
        {
            try
            {
                StringBuilder result = new StringBuilder();
                var products = JsonConvert.DeserializeObject<Dictionary<string, byte>>(productsJson);
                
                foreach (var product in products)
                {
                    result.Append($"{product.Key}, {product.Value} pcs.\n");
                }

                return result.ToString();
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public static Time ToTimeInstance(this string timeJson)
        {
            try
            {
                return JsonConvert.DeserializeObject<Time>(timeJson) as Time;
            }
            catch
            {
                return null;
            }
        }

        public static string GetModelsLocalizedErrors(this IStringLocalizer localizer, ModelStateDictionary model)
        {
            StringBuilder result = new StringBuilder();
            IEnumerable<string> errorMessages = model.Values
                .SelectMany(v => v.Errors)
                .Select(i => localizer.GetLocalizationOf(i.ErrorMessage));

            result.AppendJoin(", ", errorMessages);
            return result.ToString();
        }
        
        public static string GetLocalizationOf(this IStringLocalizer localizer, string content)
        {
            return localizer[content].Value;
        }

        public static string ConvertToAccountNumberPublicViewFormat(this string accNumber)
        {
            string hiddenDigits = "********";
            string replacable = accNumber.Substring(4, 8);
            return accNumber.Replace(replacable, hiddenDigits);
        }
    }
}
