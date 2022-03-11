using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reservation.Data.Entities
{
    
    public class PaymentData
    {
        public long Id { get; set; }

        public DateTime PaymentDate { get; set; }

        public long BankCardIdFrom { get; set; }

        public long BankAcountIdTo { get; set; }

        public decimal Amount { get; set; }

        public bool Approved { get; set; }

        public BankCard BankCard { get; set; }

        public BankAccount BankAccount { get; set; }
    }
}
