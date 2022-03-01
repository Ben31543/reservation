using System;
using System.ComponentModel.DataAnnotations;

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
