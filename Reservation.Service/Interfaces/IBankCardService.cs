using Reservation.Models.BankCard;
using Reservation.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reservation.Service.Interfaces
{
    public interface IBankCardService
    {
        Task<RequestResult> AttachCardToMemberAsync(AttachCardToMemberModel model);

        Task<RequestResult> DetachCardFromMemberAsync(long bankCardId);
    }
}
