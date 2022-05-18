using Reservation.Data.Entities;
using Reservation.Models.BankCard;
using Reservation.Models.Common;
using System.Threading.Tasks;

namespace Reservation.Service.Interfaces
{
    public interface IBankCardService
    {
        Task<RequestResult> AttachCardToMemberAsync(AttachCardToMemberModel model);

        Task<bool> CheckBankCardExistsByCardNumberAsync(string bankCardNumber);

        Task<BankCard> GetBankCardByIdAsync(long id);
    }
}
