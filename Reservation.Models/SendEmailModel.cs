using System.ComponentModel.DataAnnotations;
using Reservation.Resources.Contents;

namespace Reservation.Models
{
    public class SendEmailModel
    {
        [Required(ErrorMessage = LocalizationKeys.Errors.FieldIsRequired)]
        public string EmailFrom { get; set; }

        [Required(ErrorMessage = LocalizationKeys.Errors.FieldIsRequired)]
        public string Message { get; set; }
    }
}