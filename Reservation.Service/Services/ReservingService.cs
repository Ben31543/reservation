using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Reservation.Data;
using Reservation.Data.Entities;
using Reservation.Data.Enumerations;
using Reservation.Models.Common;
using Reservation.Models.Reserving;
using Reservation.Models.ServiceMemberBranch;
using Reservation.Resources.Contents;
using Reservation.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reservation.Service.Services
{
    public class ReservingService : IReservingService
    {
        private readonly ApplicationContext _db;
        private readonly IPaymentService _paymentService;
        private readonly ILogger _logger;
        private readonly IServiceMemberService _serviceMemberService;
        private readonly IDishService _dishService;

        public ReservingService(
            ApplicationContext db,
            IPaymentService payment,
            ILogger<ReservingService> logger,
            IServiceMemberService serviceMemberService,
            IDishService dishService)
        {
            _db = db;
            _paymentService = payment;
            _logger = logger;
            _serviceMemberService = serviceMemberService;
            _dishService = dishService;
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

            Dish dish = null;
            decimal amount = 0;
            if (model.Dishes != null)
            {
                foreach (var dishItem in model.Dishes)
                {
                    dish = await _dishService.GetDishById(dishItem.Key);
                    if (dish != null)
                    {
                        amount += dish.Price * dishItem.Value;
                    }
                }
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
                Amount = amount,
                IsActive = true
            };

            ++serviceMember.OrdersCount;
            await _db.Reservings.AddAsync(reservation);

            if (reservation.IsOnlinePayment)
            {
                await AddPaymentRequestAsync(reservation);
            }

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

        public async Task<IList<ReservableBranchModel>> GetReservableBranchesAsync(SearchForReservingModel model)
        {
            var branches = _db.ServiceMemberBranches.Include(i => i.ServiceMember).Where(i => i.IsActive).AsQueryable();
            if (!string.IsNullOrEmpty(model.ServiceMemberName))
            {
                branches = branches.Where(i => i.ServiceMember.Name.Contains(model.ServiceMemberName));
            }

            IList<ServiceMemberBranch> returnableBranches = new List<ServiceMemberBranch>();
            foreach (var branch in branches)
            {
                Dictionary<TableSchemas, byte> schemas = JsonConvert.DeserializeObject(branch.TablesSchema) as Dictionary<TableSchemas, byte>;
                if (schemas.ContainsKey(model.PersonsCount.Value) && schemas[model.PersonsCount.Value] != 0)
                {
                    returnableBranches.Add(branch);
                }
            }

            return returnableBranches.Select(i => new ReservableBranchModel
            {
                Id = i.Id,
                ServiceMemberName = i.ServiceMember.Name,
                BranchAddress = i.Address,
                LogoUrl = i.ServiceMember.LogoUrl,
                FreeTimes = _db.Reservings
                                .Where(r =>
                                           r.ServiceMemberBranchId == i.Id
                                           && r.IsActive
                                           && r.ReservationDate.Date == model.ReservingDate.Value.Date
                                           && r.ReservationDate.TimeOfDay != model.ReservingDate.Value.TimeOfDay)
                                .Select(d => d.ReservationDate)
                                .ToList()
            }).ToList();
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
                                        .ThenInclude(i => i.BankCard)
                                       .Include(i => i.ServiceMember)
                                        .ThenInclude(i => i.BankAccount)
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
