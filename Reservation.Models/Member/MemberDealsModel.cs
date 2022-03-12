using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reservation.Models.Member
{
    public class MemberDealsModel
    {
        public int ReservationId { get; set; }

        public string BranchAddress { get; set; }

        public decimal Amount { get; set; }

        public bool OnlinePayment { get; set; }

        public string ServiceMemberName { get; set; }

        public DateTime ReservingDate { get; set; }
    }
}
