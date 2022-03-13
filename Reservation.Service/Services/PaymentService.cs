using Microsoft.Extensions.Logging;
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
        private readonly ILogger _logger;

        public PaymentService(ApplicationContext db, ILogger<PaymentService> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<RequestResult> AddPaymentAsync(PaymentDataModel model)
        {
            RequestResult result = new RequestResult();

            var paymentData = new PaymentData
            {
                Amount = model.Amount,
                BankAcountIdTo = model.BankAcountTo,
                BankCardIdFrom = model.BankCardAccountFrom,
                PaymentDate = DateTime.Now
            };

            await _db.PaymentDatas.AddAsync(paymentData);

            try
            {
                paymentData.Approved = true;
                await _db.SaveChangesAsync();
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                _logger.LogError(e.Message);
                return result;
            }

            result.Succeeded = true;
            result.Value = paymentData;
            return result;
        }
    }
}
