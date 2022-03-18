using System;

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

        public bool IsActive { get; set; }
    }
}
