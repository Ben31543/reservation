using Reservation.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reservation.Models.ServiceMemberBranch
{
    public class ReservableBranchModel
    {
        public long Id { get; set; }

        public string ServiceMemberName { get; set; }

        public string BranchAddress { get; set; }

        public string LogoUrl { get; set; }

        public IList<Time> FreeTimes { get; set; }
    }
}
