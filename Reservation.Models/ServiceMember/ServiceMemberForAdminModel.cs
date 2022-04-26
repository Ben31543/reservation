using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reservation.Models.ServiceMember
{
    public class ServiceMemberForAdminModel
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public int ViewsCount { get; set; }

        public int OrdersCount { get; set; }

        public string Email { get; set; }

        public bool AcceptsOnlinePayment { get; set; }

        public string Facebook { get; set; }

        public string Instagram { get; set; }

        public string BankAccount { get; set; }

        public int BranchesCount { get; set; }
    }
}
