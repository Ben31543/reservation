using System;

namespace Reservation.Data.Entities
{
	public class BankAccount
    {
        public long Id { get; set; }

        public string AccountNumber { get; set; }

        public decimal? Balance { get; set; }

        public long BankId { get; set; }

        public long ServiceMemberId { get; set; }

        public DateTime CreationDate { get; set; }

        public Bank Bank { get; set; }

        public ServiceMember ServiceMember { get; set; }
    }
}
