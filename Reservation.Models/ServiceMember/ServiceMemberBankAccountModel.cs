using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reservation.Models.ServiceMember
{
    public class ServiceMemberBankAccountModel
    {
        [Required]
        public string AccountNumber { get; set; }

        public long BankId { get; set; }

        public long ServiceMemberId { get; set; }

        public DateTime CreationDate => DateTime.Now;
    }
}
