using Reservation.Resources.Contents;
using System.ComponentModel.DataAnnotations;

namespace Reservation.Models.BankAccount
{
    public class BankAccountAttachModel
    {
        public long? ServiceMemberId { get; set; }

        [Required(ErrorMessage = LocalizationKeys.Contents.AccountNumber + " " + LocalizationKeys.ErrorMessages.FieldIsRequired)]
        public string AccountNumber { get; set; }
    }
}
