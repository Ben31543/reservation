using Reservation.Data.Constants;
using System.ComponentModel.DataAnnotations;

namespace Reservation.Models.Common
{
    public class ResetPasswordModel
    {
        [Required(ErrorMessage = Localizations.Errors.ThisFieldIsRequired)]
        public string Login { get; set; }

        [Required(ErrorMessage = Localizations.Errors.ThisFieldIsRequired)]
        [MinLength(8), MaxLength(12)]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = Localizations.Errors.ThisFieldIsRequired)]
        public string ConfirmNewPassword { get; set; }
    }
}
