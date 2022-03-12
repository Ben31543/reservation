using Reservation.Data.Constants;
using System.ComponentModel.DataAnnotations;

namespace Reservation.Models.Common
{
    public class SignInModel
    {
        [Required(ErrorMessage = Localizations.Errors.ThisFieldIsRequired)]
        public string Login { get; set; }

        [Required(ErrorMessage = Localizations.Errors.ThisFieldIsRequired)]
        public string Password { get; set; }
    }
}
