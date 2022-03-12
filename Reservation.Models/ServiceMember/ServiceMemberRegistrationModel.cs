using Reservation.Data.Constants;
using System.ComponentModel.DataAnnotations;

namespace Reservation.Models.ServiceMember
{
    public class ServiceMemberRegistrationModel
    {
        [Required(ErrorMessage = Localizations.Errors.ThisFieldIsRequired)]
        [StringLength(50)]
        public string Name { get; set; }

        [Required(ErrorMessage = Localizations.Errors.ThisFieldIsRequired)]
        [StringLength(255)]
        public string Email { get; set; }

        [StringLength(255)]
        public string FacebookUrl { get; set; }

        [StringLength(255)]
        public string InstagramUrl { get; set; }

        [Required(ErrorMessage = Localizations.Errors.ThisFieldIsRequired)]
        public bool AcceptsOnlinePayment { get; set; }

        [Required(ErrorMessage = Localizations.Errors.ThisFieldIsRequired)]
        [MinLength(8), MaxLength(12)]
        public string Password { get; set; }

        [Required(ErrorMessage = Localizations.Errors.ThisFieldIsRequired)]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
    }
}
