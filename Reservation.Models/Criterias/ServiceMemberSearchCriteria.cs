using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reservation.Models.Criterias
{
    public class ServiceMemberSearchCriteria
    {
        public string Name { get; set; }

		public bool? AcceptsOnlinePayment { get; set; }
	}
}
