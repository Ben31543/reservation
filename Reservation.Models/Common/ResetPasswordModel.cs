using Reservation.Resources.Contents;
using System.ComponentModel.DataAnnotations;

namespace Reservation.Models.Common
{
    public class ResetPasswordModel
    {
        [Required(ErrorMessage = LocalizationKeys.Contents.Login + LocalizationKeys.Errors.FieldIsRequired)]
        public string Login { get; set; }

        [Required(ErrorMessage = LocalizationKeys.Contents.Password + LocalizationKeys.Errors.FieldIsRequired)]
        [MinLength(8), MaxLength(12)]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = LocalizationKeys.Contents.Password + LocalizationKeys.Errors.FieldIsRequired)]
        public string ConfirmNewPassword { get; set; }
    }
}
