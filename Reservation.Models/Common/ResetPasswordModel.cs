using Reservation.Resources.Contents;
using System.ComponentModel.DataAnnotations;

namespace Reservation.Models.Common
{
    public class ResetPasswordModel
    {
        [Required(ErrorMessage = LocalizationKeys.Contents.Login + " " + LocalizationKeys.ErrorMessages.FieldIsRequired)]
        public string Login { get; set; }

        [Required(ErrorMessage = LocalizationKeys.Contents.Passowrd + " " + LocalizationKeys.ErrorMessages.FieldIsRequired)]
        [MinLength(8), MaxLength(12)]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = LocalizationKeys.Contents.Passowrd + " " + LocalizationKeys.ErrorMessages.FieldIsRequired)]
        public string ConfirmNewPassword { get; set; }
    }
}
