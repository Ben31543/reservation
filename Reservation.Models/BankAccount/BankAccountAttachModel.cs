using Reservation.Resources.Contents;
using System.ComponentModel.DataAnnotations;

namespace Reservation.Models.BankAccount
{
    public class BankAccountAttachModel
    {
        public long? ServiceMemberId { get; set; }

        [Required(ErrorMessage = LocalizationKeys.ErrorMessages.ThisFieldIsRequired)]
        public string AccountNumber { get; set; }
    }
}
