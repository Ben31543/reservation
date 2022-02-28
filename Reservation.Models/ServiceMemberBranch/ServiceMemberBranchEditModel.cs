using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reservation.Models.ServiceMemberBranch
{
    public class ServiceMemberBranchEditModel
    {
        public long? Id { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public long ServiceMemberId { get; set; }

        public string Phone { get; set; }

        public TimeSpan OpenTime { get; set; }

        public TimeSpan CloseTime { get; set; }

        public string WorkDays { get; set; }

        public bool? IsActive { get; set; }

        public Dictionary<string,byte> TablesSchema { get; set; }
    }
}
