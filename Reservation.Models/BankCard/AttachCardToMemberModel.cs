using Reservation.Resources.Contents;
using System;
using System.ComponentModel.DataAnnotations;

namespace Reservation.Models.BankCard
{
    public class AttachCardToMemberModel
    {
        [Required(ErrorMessage = LocalizationKeys.Contents.MemberId + LocalizationKeys.ErrorMessages.FieldIsRequired)]
        public long MemberId { get; set; }

        [Required(ErrorMessage = LocalizationKeys.Contents.CardNumber + LocalizationKeys.ErrorMessages.FieldIsRequired)]
        [StringLength(16)]
        public string CardNumber { get; set; }

        [Required(ErrorMessage = LocalizationKeys.Contents.CVV + LocalizationKeys.ErrorMessages.FieldIsRequired)]
        [StringLength(3)]
        public string CVV { get; set; }

        [Required(ErrorMessage = LocalizationKeys.Contents.ValidThru + LocalizationKeys.ErrorMessages.FieldIsRequired)]
        public DateTime ValidThru { get; set; }

        [Required(ErrorMessage = LocalizationKeys.Contents.Owner + LocalizationKeys.ErrorMessages.FieldIsRequired)]
        public string Owner { get; set; }
    }
}
