using Newtonsoft.Json;
using Reservation.Data;
using Reservation.Data.Entities;
using Reservation.Models.Common;
using Reservation.Models.Reserving;
using Reservation.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Web.Helpers;

namespace Reservation.Service.Services
{
    public class ReservingService : IReservingService
    {
        private readonly ApplicationContext _db;
        private readonly IPaymentService _payment;
        public ReservingService(ApplicationContext db, IPaymentService payment)
        {
            _db = db;
            _payment = payment;
        }
        public async Task<RequestResult> AddReservingAsync(ReservingModel model)
        {
            RequestResult result = new RequestResult();
            var reserve = new Reserving
            {
                IsOnlinePayment = model.IsOnlinePayment,
                IsTakeOut = model.IsTakeOut,
                MemberId = model.MemberId,
                ReservationDate = model.ReservationDate,
                ServiceMemberId = model.ServiceMemberId,
                ServiceMemberBranchId = model.ServiceMemberBranchId,
                Tables = JsonConvert.SerializeObject(model.Tables),
                Dishes = JsonConvert.SerializeObject(model.Dishes),
                Notes = model.Notes,
                Amount=model.Amount
            };

            await _db.Reservings.AddAsync(reserve);

            try
            {
                await _db.SaveChangesAsync();
                result.Succeeded = true;
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                return result;
            }

            if (reserve.IsOnlinePayment)
            {
                var approvePayment = new PaymentDataModel
                {
                    Amount = reserve.Amount,
                    PaymentDate = DateTime.Now,
                    BankCardIdFrom = reserve.Member.BankCardId.Value,
                    BankAcountIdTo = reserve.ServiceMember.BankAccountId.Value
                };

                await _payment.AddPaymentAsync(approvePayment);
            }

            result.Value = reserve;
            return result;
        }
    }
}
