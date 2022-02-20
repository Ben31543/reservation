using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reservation.Models.Member
{
    public class MemberEditModel
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public DateTime BirthDate { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }
    }
}
