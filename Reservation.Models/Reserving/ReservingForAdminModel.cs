using System;

namespace Reservation.Models.Reserving
{
    public class ReservingForAdminModel
    {
        public DateTime ReservingDate { get; set; }

        public string ServiceMember { get; set; }

        public string Branch { get; set; }

        public string PayMethod { get; set; }

        public decimal Amount { get; set; }

        public string Status { get; set; }
    }
}