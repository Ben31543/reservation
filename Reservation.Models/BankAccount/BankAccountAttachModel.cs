using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reservation.Models.BankAccount
{
    public class BankAccountAttachModel
    {
        public long? ServiceMemberId { get; set; }

        [Required(ErrorMessage ="FieldIsRequired")]
        public string AccountNumber { get; set; }
    }
}
