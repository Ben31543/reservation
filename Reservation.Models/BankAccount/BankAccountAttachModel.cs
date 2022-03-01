using System.ComponentModel.DataAnnotations;

namespace Reservation.Models.BankAccount
{
    public class BankAccountAttachModel
    {
        public long? ServiceMemberId { get; set; }

        [Required(ErrorMessage ="FieldIsRequired")]
        public string AccountNumber { get; set; }
    }
}
