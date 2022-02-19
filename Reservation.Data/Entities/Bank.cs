using System.Collections.Generic;

namespace Reservation.Data.Entities
{
    public class Bank
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public ICollection<BankCard> BankCards { get; set; }

        public ICollection<BankAccount> BankAccounts { get; set; } 
    }
}
