using Reservation.Data.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reservation.Models.Member
{
    public class MemberEditModel
    {
        public long Id { get; set; }

        [Required(ErrorMessage = Localizations.Errors.ThisFieldIsRequired)]
        public string Name { get; set; }

        [Required(ErrorMessage = Localizations.Errors.ThisFieldIsRequired)]
        public string Surname { get; set; }

        [Required(ErrorMessage = Localizations.Errors.ThisFieldIsRequired)]
        public DateTime BirthDate { get; set; }

        [Required(ErrorMessage = Localizations.Errors.ThisFieldIsRequired)]
        public string Phone { get; set; }

        [Required(ErrorMessage = Localizations.Errors.ThisFieldIsRequired)]
        public string Email { get; set; }
    }
}
