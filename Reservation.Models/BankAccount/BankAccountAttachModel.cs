using Reservation.Data.Constants;
using System.ComponentModel.DataAnnotations;

namespace Reservation.Models.BankAccount
{
    public class BankAccountAttachModel
    {
        public long? ServiceMemberId { get; set; }

        [Required(ErrorMessage = Localizations.Errors.ThisFieldIsRequired)]
        public string AccountNumber { get; set; }
    }
}
