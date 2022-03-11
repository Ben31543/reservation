using Reservation.Data;
using Reservation.Data.Entities;
using Reservation.Models.Common;
using Reservation.Models.Reserving;
using Reservation.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reservation.Service.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly ApplicationContext _db;

        public PaymentService(ApplicationContext db)
        {
            _db = db;
        }

        public async Task<RequestResult> AddPaymentAsync(PaymentDataModel model)
        {
            RequestResult result = new RequestResult();

            var paymentData = new PaymentData
            {
                Amount = model.Amount,
                BankAcountIdTo = model.BankAcountIdTo,
                BankCardIdFrom = model.BankCardIdFrom,
                PaymentDate = DateTime.Now
            };

            try
            {
                await _db.PaymentDatas.AddAsync(paymentData);
                paymentData.Approved = true;
                await _db.SaveChangesAsync();
                result.Succeeded = true;
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                return result;
            }

            result.Value = paymentData;
            return result;
        }
    }
}
