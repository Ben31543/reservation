using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reservation.Models.Common
{
    public class PasswordResetModel
    {
        public long Id { get; set; }

        [Required]
        public string LogIn { get; set; }

        [Required(ErrorMessage = "FieldIsRequired")]
        [MinLength(8), MaxLength(12)]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "FieldIsRequired")]
        [Compare(nameof(NewPassword))]
        public string ConfirmNewPassword { get; set; }
    }
}
