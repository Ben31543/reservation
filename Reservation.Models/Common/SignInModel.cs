using Reservation.Resources.Contents;
using System.ComponentModel.DataAnnotations;

namespace Reservation.Models.Common
{
    public class SignInModel
    {
        [Required(ErrorMessage = LocalizationKeys.ErrorMessages.ThisFieldIsRequired)]
        public string Login { get; set; }

        [Required(ErrorMessage = LocalizationKeys.ErrorMessages.ThisFieldIsRequired)]
        public string Password { get; set; }
    }
}
