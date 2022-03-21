using Reservation.Data.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reservation.Models.Reserving
{
    public class SearchForReservingModel
    {
        public DateTime? ReservingDate { get; set; }

        public TableSchemas? PersonsCount { get; set; }

        public string ServiceMemberName { get; set; }
    }
}
