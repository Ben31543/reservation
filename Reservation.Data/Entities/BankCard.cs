using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reservation.Data.Entities
{
    public class BankCard
    {
        public long Id { get; set; }

        public string Number { get; set; }

        public DateTime ValidThru { get; set; }

        public string Owner { get; set; }

        public decimal? CurrentBalance { get; set; }

        public byte CardTypeId { get; set; }

        public long BankId { get; set; }

        public long MemberId { get; set; }

        public bool IsActive { get; set; }

        public Bank Bank { get; set; }

        public Member Member { get; set; }
    }
}
