using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Reservation.Data.Entities
{
    public class Member
    {
        [Key]
        public long Id { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        [JsonIgnore]
        public string PasswordHash { get; set; }

        public long? BankCardId { get; set; }

        public BankCard BankCard { get; set; }
    }
}
