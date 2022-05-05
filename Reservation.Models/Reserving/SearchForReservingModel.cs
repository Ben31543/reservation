using System;
using Reservation.Resources.Enumerations;

namespace Reservation.Models.Reserving
{
    public class SearchForReservingModel
    {
        public DateTime? ReservingDate { get; set; }

        public TableSchemas? PersonsCount { get; set; }

        public string ServiceMemberName { get; set; }

        public bool? HasOnlinePayment { get; set; }

        public bool? IsOpenNow { get; set; }
    }
}
