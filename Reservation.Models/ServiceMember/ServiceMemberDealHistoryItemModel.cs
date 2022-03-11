using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reservation.Models.ServiceMember
{
    public class ServiceMemberDealHistoryItemModel
    {
        public int Id { get; set; }

        public long ServiceId { get; set; }

        public string Address { get; set; }

        public decimal Amount { get; set; }

        public bool OnlinePayment { get; set; }

        public string BranchName { get; set; }

        public DateTime OrdersDate { get; set; }
    }
}
