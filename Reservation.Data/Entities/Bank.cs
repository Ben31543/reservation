using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Reservation.Data.Entities
{
    public class Bank
    {
        [Key]
        public long Id { get; set; }

        public string Name { get; set; }

        public ICollection<BankCard> BankCards { get; set; }

        public ICollection<BankAccount> BankAccounts { get; set; } 
    }
}
