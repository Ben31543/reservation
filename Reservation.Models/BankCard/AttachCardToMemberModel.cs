using Reservation.Resources.Contents;
using System;
using System.ComponentModel.DataAnnotations;

namespace Reservation.Models.BankCard
{
    public class AttachCardToMemberModel
    {
        [Required(ErrorMessage = LocalizationKeys.ErrorMessages.ThisFieldIsRequired)]
        public long MemberId { get; set; }

        [Required(ErrorMessage = LocalizationKeys.ErrorMessages.ThisFieldIsRequired)]
        [StringLength(16)]
        public string CardNumber { get; set; }

        [Required(ErrorMessage = LocalizationKeys.ErrorMessages.ThisFieldIsRequired)]
        [StringLength(3)]
        public string CVV { get; set; }

        [Required(ErrorMessage = LocalizationKeys.ErrorMessages.ThisFieldIsRequired)]
        public DateTime ValidThru { get; set; }

        [Required(ErrorMessage = LocalizationKeys.ErrorMessages.ThisFieldIsRequired)]
        public string Owner { get; set; }
    }
}
