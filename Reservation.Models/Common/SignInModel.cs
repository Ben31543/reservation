using Reservation.Resources.Contents;
using System.ComponentModel.DataAnnotations;

namespace Reservation.Models.Common
{
    public class SignInModel
    {
        [Required(ErrorMessage = LocalizationKeys.Contents.Login + LocalizationKeys.ErrorMessages.FieldIsRequired)]
        public string Login { get; set; }

        [Required(ErrorMessage = LocalizationKeys.Contents.Password + LocalizationKeys.ErrorMessages.FieldIsRequired)]
        public string Password { get; set; }
    }
}
