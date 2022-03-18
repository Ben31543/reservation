using Reservation.Resources.Contents;
using System.ComponentModel.DataAnnotations;

namespace Reservation.Models.ServiceMember
{
    public class ServiceMemberRegistrationModel
    {
        [Required(ErrorMessage = LocalizationKeys.Contents.Name + " " + LocalizationKeys.ErrorMessages.FieldIsRequired)]
        [StringLength(50)]
        public string Name { get; set; }

        [Required(ErrorMessage = LocalizationKeys.Contents.Email + " " + LocalizationKeys.ErrorMessages.FieldIsRequired)]
        [StringLength(255)]
        public string Email { get; set; }

        [StringLength(255)]
        public string FacebookUrl { get; set; }

        [StringLength(255)]
        public string InstagramUrl { get; set; }

        [Required(ErrorMessage = LocalizationKeys.Contents.AcceptsOnlinePayment + " " + LocalizationKeys.ErrorMessages.FieldIsRequired)]
        public bool AcceptsOnlinePayment { get; set; }

        [Required(ErrorMessage = LocalizationKeys.Contents.Passowrd + " " + LocalizationKeys.ErrorMessages.FieldIsRequired)]
        [MinLength(8), MaxLength(12)]
        public string Password { get; set; }

        [Required(ErrorMessage = LocalizationKeys.Contents.Passowrd + " " + LocalizationKeys.ErrorMessages.FieldIsRequired)]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
    }
}
