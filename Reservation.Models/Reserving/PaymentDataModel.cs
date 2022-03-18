using System;

namespace Reservation.Models.Reserving
{
    public class PaymentDataModel
    {
        public long Id { get; set; }

        public DateTime PaymentDate { get; set; }

        public long BankCardId { get; set; }

        public long BankAccountId { get; set; }

        public string BankCardAccountFrom { get; set; }

        public string BankAcountTo { get; set; }

        public decimal Amount { get; set; }

        public bool Approved { get; set; }
    }
}
