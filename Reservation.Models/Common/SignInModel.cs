using System.ComponentModel.DataAnnotations;

namespace Reservation.Models.Common
{
    public class SignInModel
    {
        [Required]
        public string LogIn { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
