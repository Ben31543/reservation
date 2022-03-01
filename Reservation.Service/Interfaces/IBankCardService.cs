using Reservation.Models.BankCard;
using Reservation.Models.Common;
using System.Threading.Tasks;

namespace Reservation.Service.Interfaces
{
    public interface IBankCardService
    {
        Task<RequestResult> AttachCardToMemberAsync(AttachCardToMemberModel model);

        Task<RequestResult> DetachCardFromMemberAsync(long bankCardId);
    }
}
