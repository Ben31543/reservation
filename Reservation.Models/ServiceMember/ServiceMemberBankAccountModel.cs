using System;
using System.ComponentModel.DataAnnotations;

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
