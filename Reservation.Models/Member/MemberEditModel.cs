using Reservation.Resources.Contents;
using System.ComponentModel.DataAnnotations;

namespace Reservation.Models.Member
{
    public class MemberEditModel
    {
        public long Id { get; set; }

        [Required(ErrorMessage = LocalizationKeys.Contents.Name + LocalizationKeys.Errors.FieldIsRequired)]
        public string Name { get; set; }

        [Required(ErrorMessage = LocalizationKeys.Contents.Surname + LocalizationKeys.Errors.FieldIsRequired)]
        public string Surname { get; set; }

        [Required(ErrorMessage = LocalizationKeys.Contents.Phone + LocalizationKeys.Errors.FieldIsRequired)]
        public string Phone { get; set; }

        [Required(ErrorMessage = LocalizationKeys.Contents.Email + LocalizationKeys.Errors.FieldIsRequired)]
        public string Email { get; set; }
    }
}
