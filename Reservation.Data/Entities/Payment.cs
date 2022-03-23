using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reservation.Data.Entities
{
    public class Payment
    {
        public long Id { get; set; }

        public DateTime PaymentDate { get; set; }

        public string BankCardIdFrom { get; set; }

        public string BankAcountIdTo { get; set; }

        public decimal Amount { get; set; }

        public bool Approved { get; set; }
    }
}
