using Reservation.Models.Common;
using Reservation.Models.Reserving;
using Reservation.Models.ServiceMemberBranch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reservation.Service.Interfaces
{
    public interface IReservingService
    {
        Task<RequestResult> AddReservingAsync(ReservingModel model);

        Task<RequestResult> CancelReservingAsync(long id);

        Task<IList<ReservableBranchModel>> GetReservableBranchesAsync(SearchForReservingModel model);
    }
}
