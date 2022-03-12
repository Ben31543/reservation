using Reservation.Resources.Contents;
using System.ComponentModel.DataAnnotations;

namespace Reservation.Models.Member
{
    public class MemberEditModel
    {
        public long Id { get; set; }

        [Required(ErrorMessage = LocalizationKeys.ErrorMessages.ThisFieldIsRequired)]
        public string Name { get; set; }

        [Required(ErrorMessage = LocalizationKeys.ErrorMessages.ThisFieldIsRequired)]
        public string Surname { get; set; }

        [Required(ErrorMessage = LocalizationKeys.ErrorMessages.ThisFieldIsRequired)]
        public string Phone { get; set; }

        [Required(ErrorMessage = LocalizationKeys.ErrorMessages.ThisFieldIsRequired)]
        public string Email { get; set; }
    }
}
