using Reservation.Data.Constants;
using System;
using System.ComponentModel.DataAnnotations;

namespace Reservation.Models.BankCard
{
    public class AttachCardToMemberModel
    {
        [Required(ErrorMessage = Localizations.Errors.ThisFieldIsRequired)]
        public long MemberId { get; set; }

        [Required(ErrorMessage = Localizations.Errors.ThisFieldIsRequired)]
        [StringLength(16)]
        public string CardNumber { get; set; }

        [Required(ErrorMessage = Localizations.Errors.ThisFieldIsRequired)]
        [StringLength(3)]
        public string CVV { get; set; }

        [Required(ErrorMessage = Localizations.Errors.ThisFieldIsRequired)]
        public DateTime ValidThru { get; set; }

        [Required(ErrorMessage = Localizations.Errors.ThisFieldIsRequired)]
        public string Owner { get; set; }
    }
}
