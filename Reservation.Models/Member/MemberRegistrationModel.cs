using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reservation.Models.Member
{
    public class MemberRegistrationModel
    {
        [Required(ErrorMessage ="FieldIsRequired")]
        [StringLength(20)]
        public string Name { get; set; }

        [Required(ErrorMessage = "FieldIsRequired")]
        [StringLength(20)]
        public string SurName { get; set; }

        [Required(ErrorMessage = "FieldIsRequired")]
        public DateTime BirthDate { get; set; }

        [Required(ErrorMessage = "FieldIsRequired")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "FieldIsRequired")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "FieldIsRequired")]
        [MinLength(8), MaxLength(12)]
        public string Password { get; set; }

        [Required(ErrorMessage = "FieldIsRequired")]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
    }
}
