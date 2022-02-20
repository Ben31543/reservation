using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reservation.Models.BankCard
{
    public class AttachCardToMemberModel
    {
        [Required]
        public long MemberId { get; set; }

        [Required]
        [StringLength(16)]
        public string CardNumber { get; set; }

        [Required]
        [StringLength(3)]
        public string CVV { get; set; }

        [Required]
        public DateTime ValidThru { get; set; }

        [Required]
        public string Owner { get; set; }
    }
}
