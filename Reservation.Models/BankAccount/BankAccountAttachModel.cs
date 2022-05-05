using Reservation.Resources.Contents;
using System.ComponentModel.DataAnnotations;

namespace Reservation.Models.BankAccount
{
    public class BankAccountAttachModel
    {
        public long? ServiceMemberId { get; set; }

        [Required(ErrorMessage = LocalizationKeys.Contents.AccountNumber + LocalizationKeys.Errors.FieldIsRequired)]
        public string AccountNumber { get; set; }

        [Required(ErrorMessage = LocalizationKeys.Contents.Name + LocalizationKeys.Errors.FieldIsRequired)]
        public string Owner { get; set; }
    }
}
