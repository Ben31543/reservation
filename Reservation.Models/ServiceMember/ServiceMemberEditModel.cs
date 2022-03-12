using Reservation.Resources.Contents;
using System.ComponentModel.DataAnnotations;

namespace Reservation.Models.ServiceMember
{
    public class ServiceMemberEditModel
    {
        public long? Id { get; set; }

        [Required(ErrorMessage = LocalizationKeys.ErrorMessages.ThisFieldIsRequired)]
        [StringLength(50)]
        public string Name { get; set; }

        [Required(ErrorMessage = LocalizationKeys.ErrorMessages.ThisFieldIsRequired)]
        [StringLength(255)]
        public string Email { get; set; }

        [StringLength(255)]
        public string FacebookUrl { get; set; }

        [StringLength(255)]
        public string InstagramUrl { get; set; }

        public string ImageUrl { get; set; }

        public string LogoUrl { get; set; }

        [Required(ErrorMessage = LocalizationKeys.ErrorMessages.ThisFieldIsRequired)]
        public bool AcceptsOnlinePayment { get; set; }
    }
}
