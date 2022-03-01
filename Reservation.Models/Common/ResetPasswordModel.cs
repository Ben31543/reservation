using System.ComponentModel.DataAnnotations;

namespace Reservation.Models.Common
{
    public class ResetPasswordModel
    {
        public long Id { get; set; }

        [Required]
        public string Login { get; set; }

        [Required(ErrorMessage = "FieldIsRequired")]
        [MinLength(8), MaxLength(12)]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "FieldIsRequired")]
        [Compare(nameof(NewPassword))]
        public string ConfirmNewPassword { get; set; }
    }
}
