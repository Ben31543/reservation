using Reservation.Resources.Contents;
using System.ComponentModel.DataAnnotations;

namespace Reservation.Models.Member
{
    public class MemberEditModel
    {
        public long Id { get; set; }

        [Required(ErrorMessage = LocalizationKeys.Contents.Name + " " + LocalizationKeys.ErrorMessages.FieldIsRequired)]
        public string Name { get; set; }

        [Required(ErrorMessage = LocalizationKeys.Contents.Surname + " " + LocalizationKeys.ErrorMessages.FieldIsRequired)]
        public string Surname { get; set; }

        [Required(ErrorMessage = LocalizationKeys.Contents.Phone + " " + LocalizationKeys.ErrorMessages.FieldIsRequired)]
        public string Phone { get; set; }

        [Required(ErrorMessage = LocalizationKeys.Contents.Email + " " + LocalizationKeys.ErrorMessages.FieldIsRequired)]
        public string Email { get; set; }
    }
}
