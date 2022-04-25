using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reservation.Models.ServiceMemberBranch
{
    public class ServiceMemberBranchForAdminModel
    {
        public string ServiceMemberName { get; set; }

        public string Address { get; set; }

        public string Phone { get; set; }

        public string WorkingHours { get; set; }

        public bool IsActive { get; set; }
    }
}
