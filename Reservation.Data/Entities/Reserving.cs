using System;

namespace Reservation.Data.Entities
{
    public class Reserving
    {
        public long Id { get; set; }

        public DateTime ReservationDate { get; set; }

        public long MemberId { get; set; }

        public long ServiceMemberBranchId { get; set; }

        public long ServiceMemberId { get; set; }

        public long? BankCardFromId { get; set; }

        public long? BankAccountToId { get; set; }

        public decimal Amount { get; set; }

        public bool IsOnlinePayment { get; set; }

        public Member Member { get; set; }

        public ServiceMember ServiceMember { get; set; }

        public ServiceMemberBranch serviceMemberBranch { get; set; }

        public BankCard BankCard { get; set; }

        public BankAccount BankAccount { get; set; }
    }
}