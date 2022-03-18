using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Reservation.Data;
using Reservation.Data.Entities;
using Reservation.Models.Common;
using Reservation.Models.Reserving;
using Reservation.Resources.Contents;
using Reservation.Service.Interfaces;
using System;
using System.Threading.Tasks;

namespace Reservation.Service.Services
{
    public class ReservingService : IReservingService
    {
        private readonly ApplicationContext _db;
        private readonly IPaymentService _paymentService;
        private readonly ILogger _logger;
        private readonly IServiceMemberService _serviceMemberService;

        public ReservingService(
            ApplicationContext db,
            IPaymentService payment,
            ILogger<ReservingService> logger,
            IServiceMemberService serviceMemberService)
        {
            _db = db;
            _paymentService = payment;
            _logger = logger;
            _serviceMemberService = serviceMemberService;
        }

        public async Task<RequestResult> AddReservingAsync(ReservingModel model)
        {
            RequestResult result = new RequestResult();
            var serviceMember = await _serviceMemberService.GetServiceMemberByIdAsync(model.ServiceMemberId);
            if (serviceMember == null)
            {
                result.Message = LocalizationKeys.ErrorMessages.ServiceMemberDoesNotExist;
                return result;
            }

            var reservation = new Reserving
            {
                IsOnlinePayment = model.IsOnlinePayment,
                IsTakeOut = model.IsTakeOut,
                MemberId = model.MemberId,
                ReservationDate = model.ReservationDate,
                ServiceMemberId = serviceMember.Id,
                ServiceMemberBranchId = model.ServiceMemberBranchId,
                Tables = JsonConvert.SerializeObject(model.Tables),
                Dishes = JsonConvert.SerializeObject(model.Dishes),
                Notes = model.Notes,
                Amount = model.Amount,
                IsActive = true
            };

            ++serviceMember.OrdersCount;
            await _db.Reservings.AddAsync(reservation);

            try
            {
                await _db.SaveChangesAsync();
                result.Succeeded = true;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                result.Message = e.Message;
                return result;
            }

            if (reservation.IsOnlinePayment)
            {
                await AddPaymentRequestAsync(reservation);
            }

            return result;
        }

        public async Task<RequestResult> CancelReservingAsync(long id)
        {
            RequestResult result = new RequestResult();

            var reserving = await _db.Reservings.FirstOrDefaultAsync(i => i.Id == id);
            if (reserving == null)
            {
                result.Message = LocalizationKeys.ErrorMessages.ReservationNotFound;
                return result;
            }

            reserving.IsActive = false;

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                result.Message = e.Message;
                return result;
            }

            if (reserving.IsOnlinePayment)
            {
                await RequestRefundAsync(reserving);
            }

            result.Succeeded = true;
            return result;
        }

        private async Task AddPaymentRequestAsync(Reserving reserving)
        {
            var reserveData = await _db.Reservings
                                       .Include(i => i.Member)
                                        .ThenInclude(i => i.BankCard)
                                       .Include(i => i.ServiceMember)
                                         .ThenInclude(i => i.BankAccount)
                                       .Include(i => i.ServiceMemberBranch)
                                       .FirstOrDefaultAsync(i => i.Id == reserving.Id);

            if (reserveData == null)
            {
                return;
            }

            var paymentData = new PaymentDataModel
            {
                Amount = reserving.Amount,
                PaymentDate = DateTime.Now,
                BankCardAccountFrom = reserving.Member.BankCard.Number,
                BankAcountTo = reserving.ServiceMember.BankAccount.AccountNumber,
                BankAccountId = reserving.ServiceMember.BankAccountId.Value,
                BankCardId = reserving.Member.BankCardId.Value
            };

            await _paymentService.AddPaymentDataAsync(paymentData);
        }

        private async Task RequestRefundAsync(Reserving reserving)
        {
            var reserveData = await _db.Reservings
                                       .Include(i => i.Member)
                                        .ThenInclude(i=>i.BankCard)
                                       .Include(i => i.ServiceMember)
                                        .ThenInclude(i=>i.BankAccount)
                                       .FirstOrDefaultAsync(i => i.Id == reserving.Id);

            if (reserveData == null)
            {
                return;
            }

            await _paymentService.RequestRefundAsync(
                reserving.ServiceMember.BankAccount.Id,
                reserving.Member.BankCard.Id,
                reserving.Amount);
        }
    }
}
