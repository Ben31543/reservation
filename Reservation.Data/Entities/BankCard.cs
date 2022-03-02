using System;
using System.ComponentModel.DataAnnotations;

namespace Reservation.Data.Entities
{
	public class BankCard
    {
        [Key]
        public long Id { get; set; }

        public string Number { get; set; }

        public DateTime ValidThru { get; set; }

        public string Owner { get; set; }

        public decimal? CurrentBalance { get; set; }

        public long BankId { get; set; }

        public string CVV { get; set; }

        public bool IsAttached { get; set; }

        public Bank Bank { get; set; }

        public Member Member { get; set; }
    }
}
