using Reservation.Resources.Contents;
using System.ComponentModel.DataAnnotations;
using Reservation.Models.Common;

namespace Reservation.Models.ServiceMember
{
    public class ServiceMemberEditModel
    {
        public long? Id { get; set; }

        [Required(ErrorMessage = LocalizationKeys.Contents.Name + LocalizationKeys.Errors.FieldIsRequired)]
        [StringLength(50)]
        public string Name { get; set; }

        [Required(ErrorMessage = LocalizationKeys.Contents.Email + LocalizationKeys.Errors.FieldIsRequired)]
        [StringLength(255)]
        public string Email { get; set; }

        [StringLength(255)]
        public string FacebookUrl { get; set; }

        [StringLength(255)]
        public string InstagramUrl { get; set; }

        public string ImageUrl { get; set; }

        public string LogoUrl { get; set; }

        [Required(ErrorMessage = LocalizationKeys.Contents.AcceptsOnlinePayment + LocalizationKeys.Errors.FieldIsRequired)]
        public bool AcceptsOnlinePayment { get; set; }

        public SaveImageModel ImageModel { get; set; }
    }
}
