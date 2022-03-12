using Reservation.Resources.Contents;
using System;
using System.ComponentModel.DataAnnotations;

namespace Reservation.Models.ServiceMember
{
    public class ServiceMemberBankAccountModel
    {
        [Required(ErrorMessage = LocalizationKeys.ErrorMessages.ThisFieldIsRequired)]
        public string AccountNumber { get; set; }

        public long BankId { get; set; }

        public long ServiceMemberId { get; set; }

        public DateTime CreationDate => DateTime.Now;
    }
}
