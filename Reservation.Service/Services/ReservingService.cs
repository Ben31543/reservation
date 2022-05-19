using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Reservation.Data;
using Reservation.Data.Entities;
using Reservation.Models.Common;
using Reservation.Models.Reserving;
using Reservation.Models.ServiceMemberBranch;
using Reservation.Resources.Contents;
using Reservation.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Reservation.Resources.Constants;
using Reservation.Resources.Enumerations;
using Reservation.Service.Helpers;

namespace Reservation.Service.Services
{
    public class ReservingService : IReservingService
    {
        private readonly ApplicationContext _db;
        private readonly IPaymentService _paymentService;
        private readonly ILogger _logger;
        private readonly IServiceMemberService _serviceMemberService;
        private readonly IServiceMemberBranchService _serviceMemberBranchService;
        private readonly IDishService _dishService;

        public ReservingService(
            ApplicationContext db,
            IPaymentService payment,
            ILogger<ReservingService> logger,
            IServiceMemberService serviceMemberService,
            IDishService dishService,
            IServiceMemberBranchService serviceMemberBranchService)
        {
            _db = db;
            _paymentService = payment;
            _logger = logger;
            _serviceMemberService = serviceMemberService;
            _dishService = dishService;
            _serviceMemberBranchService = serviceMemberBranchService;
        }

        public async Task<RequestResult> AddReservingAsync(ReservingModel model)
        {
            RequestResult result = new RequestResult();
            var serviceMember = await _serviceMemberService.GetServiceMemberAsync(model.ServiceMemberId);
            if (serviceMember == null)
            {
                result.Message = LocalizationKeys.Errors.ServiceMemberDoesNotExist;
                return result;
            }

            if (model.IsOnlinePayment && !serviceMember.AcceptsOnlinePayment)
            {
                result.Message = LocalizationKeys.Errors.ServiceMemberDoesNotAcceptOnlinePayments;
                return result;
            }
            
            Dish dish = null;
            decimal amount = 0;
            Dictionary<string, byte> dishes = new Dictionary<string, byte>();
            if (model.Dishes != null && model.Dishes.Any())
            {
                foreach (var dishItem in model.Dishes)
                {
                    dish = await _dishService.GetDishById(dishItem.Key);
                    if (dish != null)
                    {
                        amount += dish.Price * dishItem.Value;
                        dishes.Add(dish.Name, dishItem.Value);
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
                Tables = JsonConvert.SerializeObject(model.Tables.ToString()),
                Dishes = JsonConvert.SerializeObject(dishes),
                Notes = model.Notes,
                Amount = amount,
                IsActive = true
            };
           
            ++serviceMember.OrdersCount;
            await _db.Reservings.AddAsync(reservation);

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
            
            if (reservation.IsOnlinePayment && serviceMember.AcceptsOnlinePayment)
            {
                await AddPaymentRequestAsync(reservation.Id);
            }

            await EmailSender.SendEmailAboutReservationAsync(serviceMember.Email, reservation, serviceMember);
            
            result.Succeeded = true;
            return result;
        }

        public async Task<RequestResult> CancelReservingAsync(long id)
        {
            RequestResult result = new RequestResult();

            var reserving = await _db.Reservings.FirstOrDefaultAsync(i => i.Id == id);
            if (reserving == null)
            {
                result.Message = LocalizationKeys.Errors.ReservationNotFound;
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

            await EmailSender.SendEmailAboutReservationCancelAsync(reserving);
            return result;
        }

        public async Task<IList<ReservableBranchModel>> GetReservableBranchesAsync(SearchForReservingModel model, bool needFreeTimes)
        {
            var branches = _db.ServiceMemberBranches.Include(i => i.ServiceMember).Where(i => i.IsActive).AsQueryable();
            if (!string.IsNullOrEmpty(model.ServiceMemberName))
            {
                branches = branches.Where(i => i.ServiceMember.Name.Contains(model.ServiceMemberName));
            }

            if (model.HasOnlinePayment.HasValue)
            {
                branches = branches.Where(i => i.ServiceMember.AcceptsOnlinePayment == model.HasOnlinePayment);
            }

            if (model.IsOpenNow.HasValue)
            {
                branches = branches.Where(i => 
                        i.OpenTime.ToTimeInstance().GetHour() >= CommonConstants.CurrentHour
                     && i.CloseTime.ToTimeInstance().GetHour() <= CommonConstants.CurrentHour);
            }
            
            IList<ServiceMemberBranch> returnableBranches = new List<ServiceMemberBranch>();
            foreach (var branch in branches)
            {
                Dictionary<TableSchemas, byte> schemas = JsonConvert.DeserializeObject<Dictionary<TableSchemas, byte>>(branch.TablesSchema);
                if (schemas == null)
                {
                    break;
                }
                
                foreach (var item in schemas)
                {
                    if (item.Key == model.PersonsCount.GetValueOrDefault() && item.Key != 0)
                    {
                        returnableBranches.Add(branch);
                    }
                }
            }

            return returnableBranches.Select(i => new ReservableBranchModel
            {
                Id = i.Id,
                ServiceMemberName = i.ServiceMember.Name,
                ServiceMemberId = i.ServiceMemberId,
                BranchAddress = i.Address,
                LogoUrl = i.ServiceMember.LogoUrl,
                FreeTimes = needFreeTimes ? GetFreeTimes(i, model.ReservingDate.Value) : null
            }).ToList();
        }

        public async Task<IList<MemberReservingForAdminModel>> GetReservingsForAdminAsync()
        {
            return await _db.Reservings
                .Include(i => i.ServiceMember)
                .Include(i => i.ServiceMemberBranch)
                .Select(r => new MemberReservingForAdminModel
                {
                    ReservingDate = r.ReservationDate,
                    ServiceMember = r.ServiceMember.Name,
                    Branch = r.ServiceMemberBranch.Address,
                    Status = r.IsActive && r.ReservationDate > DateTime.Now
                        ? "Completed"
                        : r.IsActive
                            ? "Completed"
                            : "Cancelled", ////if reserving date haven't come yet, the reserving is active, otherwise is completed
                    Amount = r.Amount,
                    Table = r.Tables,
                    OrderedProducts = r.Dishes.ToProductsDisplayFormat(),
                    PayMethod = r.IsOnlinePayment ? "Online" : "Cash"
                }).ToListAsync();
        }

        private async Task AddPaymentRequestAsync(long reservingId)
        {
            var reserveData = await _db.Reservings
                                       .Include(i => i.Member)
                                        .ThenInclude(i => i.BankCard)
                                       .Include(i => i.ServiceMember)
                                         .ThenInclude(i => i.BankAccount)
                                       .Include(i => i.ServiceMemberBranch)
                                       .FirstOrDefaultAsync(i => i.Id == reservingId);

            if (reserveData == null)
            {
                return;
            }

            var paymentData = new PaymentDataModel
            {
                Amount = reserveData.Amount,
                PaymentDate = DateTime.Now,
                BankCardAccountFrom = reserveData.Member.BankCard.Number,
                BankAcountTo = reserveData.ServiceMember.BankAccount.AccountNumber,
                BankAccountId = reserveData.ServiceMember.BankAccountId.Value,
                BankCardId = reserveData.Member.BankCardId.Value
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

        private List<Time> GetFreeTimes(ServiceMemberBranch branch, DateTime date)
        {
            var reservings = _db.Reservings.Where(i => i.IsActive
                                    && i.ServiceMemberBranchId == branch.Id
                                    && i.ReservationDate.Date == date.Date)
                                    .Select(i => i.ReservationDate.Hour).ToList();

            var openTime = JsonConvert.DeserializeObject<Time>(branch.OpenTime);
            var closeTime = JsonConvert.DeserializeObject<Time>(branch.CloseTime);

            List<Time> allTimes = new List<Time>();
            if (openTime == null || closeTime == null)
            {
                return allTimes;
            }

            for (int i = openTime.GetHour(); i <= closeTime.GetHour() - 1; i++)
            {
                if (!reservings.Contains(i))
                {
                    allTimes.Add(new Time
                    {
                        Hour = i.ToString(),
                        Minute = "00"
                    });
                }
            }

            return allTimes;
        }
    }
}
