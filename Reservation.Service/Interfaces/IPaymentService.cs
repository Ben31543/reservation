using Reservation.Data.Entities;
using Reservation.Models.Common;
using Reservation.Models.Reserving;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reservation.Service.Interfaces
{
    public interface IPaymentService
    {
        Task<RequestResult> AddPaymentDataAsync(PaymentDataModel model);

        Task<RequestResult> RequestRefundAsync(long fromId, long toId, decimal amount);
    }
}
