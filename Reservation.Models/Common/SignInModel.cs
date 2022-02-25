using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reservation.Models.Common
{
    public class SignInModel
    {
        [Required]
        public string LogIn { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
