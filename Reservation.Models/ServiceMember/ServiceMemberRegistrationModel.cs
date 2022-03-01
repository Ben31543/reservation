using System.ComponentModel.DataAnnotations;

namespace Reservation.Models.ServiceMember
{
    public class ServiceMemberRegistrationModel
    {
        [Required(ErrorMessage = "ServiceMemberMustHaveAName")]
        [StringLength(50)]
        public string Name { get; set; }

        [Required(ErrorMessage ="ServiceMemberMustHaveAnEmail")]
        [StringLength(255)]
        public string Email { get; set; }

        [StringLength(255)]
        public string FacebookUrl { get; set; }

        [StringLength(255)]
        public string InstagramUrl { get; set; }

        [Required]
        public bool AcceptsOnlinePayment { get; set; }

        [Required(ErrorMessage = "FieldIsRequired")]
        [MinLength(8), MaxLength(12)]
        public string Password { get; set; }

        [Required(ErrorMessage = "FieldIsRequired")]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
    }
}
