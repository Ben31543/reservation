using System;

namespace Reservation.Data.Entities
{
    public class Member
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public DateTime BirthDate { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public string PasswordHash { get; set; }

        public long? BankCardId { get; set; }

        public BankCard BankCard { get; set; }
    }
}
