using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Reservation.Data;
using Reservation.Data.Entities;
using Reservation.Models.BankAccount;
using Reservation.Models.Common;
using Reservation.Models.Criterias;
using Reservation.Models.ServiceMember;
using Reservation.Resources.Contents;
using Reservation.Service.Helpers;
using Reservation.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Reservation.Resources.Constants;

namespace Reservation.Service.Services
{
    public class ServiceMemberService : IServiceMemberService
    {
        private readonly ILogger _logger;
        private readonly ApplicationContext _db;
        private readonly IBankAccountService _bankAccService;
        private readonly IImageSavingService _imageSavingService;

        public ServiceMemberService(
            ApplicationContext db,
            IBankAccountService bankAccService,
            ILogger<ServiceMemberService> logger,
            IImageSavingService imageSavingService)
        {
            _db = db;
            _bankAccService = bankAccService;
            _logger = logger;
            _imageSavingService = imageSavingService;
        }

        public async Task<ServiceMember> GetServiceMemberByIdAsync(long id, bool isMember = false)
        {
            var serviceMember = await _db.ServiceMembers.FirstOrDefaultAsync(i => i.Id == id);
            if (isMember)
            {
                ++serviceMember.ViewsCount;
                await _db.SaveChangesAsync();
            }

            return serviceMember;
        }

        public async Task<RequestResult> RegisterServiceMemberAsync(ServiceMemberRegistrationModel model)
        {
            RequestResult result = new RequestResult();
            await _db.ServiceMembers.AddAsync(new ServiceMember
            {
                Name = model.Name,
                Email = model.Email,
                FacebookUrl = model.FacebookUrl,
                InstagramUrl = model.InstagramUrl,
                PasswordHash = model.Password.ToHashedPassword(),
                AcceptsOnlinePayment = model.AcceptsOnlinePayment
            });

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

        public async Task<RequestResult> ResetPasswordAsync(ResetPasswordModel model)
        {
            RequestResult result = new RequestResult();
            var serviceMember = await _db.ServiceMembers.SingleOrDefaultAsync(i => i.Email == model.Login);
            if (serviceMember == null)
            {
                result.Message = LocalizationKeys.ErrorMessages.ServiceMemberDoesNotExist;
                return result;
            }

            serviceMember.PasswordHash = model.NewPassword.ToHashedPassword();

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

        public async Task<RequestResult> UpdateServiceMemberInfoAsync(ServiceMemberEditModel model)
        {
            RequestResult result = new RequestResult();
            var serviceMember = await _db.ServiceMembers.FirstOrDefaultAsync(i => i.Id == model.Id);
            if (serviceMember == null)
            {
                result.Message = LocalizationKeys.ErrorMessages.ServiceMemberDoesNotExist;
                result.Value = model.Id;
                return result;
            }

            serviceMember.Name = model.Name;
            serviceMember.AcceptsOnlinePayment = model.AcceptsOnlinePayment;
            serviceMember.InstagramUrl = model.InstagramUrl;
            serviceMember.FacebookUrl = model.FacebookUrl;
            serviceMember.LogoUrl = model.LogoUrl;
            serviceMember.Email = model.Email;

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

            result.Value = serviceMember;
            return result;
        }

        public async Task<RequestResult> VerifyServiceMemberAsync(SignInModel model)
        {
            RequestResult result = new RequestResult();

            var serviceMember = await _db.ServiceMembers
                .FirstOrDefaultAsync(i =>
                    i.Email == model.Login && i.PasswordHash == model.Password.ToHashedPassword());
            if (serviceMember == null)
            {
                result.Message = LocalizationKeys.ErrorMessages.WrongCredientials;
                result.Value = model;
                return result;
            }

            result.Succeeded = true;
            return result;
        }

        public async Task<RequestResult> AddBankAccountAsync(BankAccountAttachModel model)
        {
            RequestResult result = new RequestResult();
            var serviceMember = await GetServiceMemberByIdAsync(model.ServiceMemberId.Value);
            if (serviceMember == null)
            {
                result.Message = LocalizationKeys.ErrorMessages.ServiceMemberDoesNotExist;
                return result;
            }

            var attachResult = await _bankAccService.AttachBankAccountToServiceMemberAsync(model);
            if (!attachResult.Succeeded)
            {
                return attachResult;
            }

            try
            {
                serviceMember.BankAccountId = (long)attachResult.Value;
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

        public async Task<RequestResult> DetachBankAccountAsync(long serviceMemberId, long bankAccountId)
        {
            var result = new RequestResult();
            var serviceMember = await GetServiceMemberByIdAsync(serviceMemberId);
            if (serviceMember == null)
            {
                result.Message = LocalizationKeys.ErrorMessages.ServiceMemberDoesNotExist;
                return result;
            }

            var bankAccount = await _bankAccService.GetBankAccountInfoAsync(bankAccountId);
            if (bankAccount == null)
            {
                result.Message = LocalizationKeys.ErrorMessages.BankAccountNotAttachedToServiceMember;
                return result;
            }

            var detachResult = await _bankAccService.DetachServiceMemberFromBankAccountAsync(bankAccountId);
            if (detachResult.Succeeded)
            {
                serviceMember.BankAccountId = null;
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

        public async Task<List<ServiceMemberDealHistoryItemModel>> GetServiceMemberDealsHistoryAsync(
            long serviceMemberId)
        {
            var deals = new List<ServiceMemberDealHistoryItemModel>();

            var serviceMember = await GetServiceMemberByIdAsync(serviceMemberId);
            if (serviceMember == null)
            {
                return deals;
            }

            deals = await _db.Reservings.Include(i => i.ServiceMemberBranch)
                .Where(i => i.Id == serviceMemberId)
                .Select(i => new ServiceMemberDealHistoryItemModel
                {
                    Amount = i.Amount,
                    OrdersDate = i.ReservationDate,
                    BranchName = i.ServiceMemberBranch.Name,
                    Address = i.ServiceMemberBranch.Address,
                    OnlinePayment = i.IsOnlinePayment,
                    IsActive = i.IsActive
                }).ToListAsync();
            return deals;
        }

        public async Task<List<ServiceMember>> GetServiceMembersAsync(ServiceMemberSearchCriteria criteria)
        {
            var serviceMembers = _db.ServiceMembers.AsNoTracking().AsQueryable();

            if (!string.IsNullOrEmpty(criteria.Name))
            {
                serviceMembers = serviceMembers.Where(i => i.Name.Contains(criteria.Name));
            }

            if (criteria.AcceptsOnlinePayment.HasValue)
            {
                serviceMembers =
                    serviceMembers.Where(i => i.AcceptsOnlinePayment == criteria.AcceptsOnlinePayment.Value);
            }

            return await serviceMembers.ToListAsync();
        }

        public async Task<RequestResult> SaveServiceMemberImageAsync(SaveImageModel model)
        {
            var result = new RequestResult();

            var serviceMember = await GetServiceMemberByIdAsync(model.Id);
            if (serviceMember == null)
            {
                result.Message = LocalizationKeys.ErrorMessages.ServiceMemberDoesNotExist;
                return result;
            }

            var image = await ImageConstructorService.ConstructImageForSaveAsync(model.Image, ImageConstructorService.ConstructFilePathFor(model.ResourceType.Value, serviceMember.Id));
            if (image == null)
            {
                result.Message = LocalizationKeys.ErrorMessages.ErrorWhileParsingImage;
                return result;
            }

            var imageSavingResult = await _imageSavingService.SaveImageAsync(image);

            if (imageSavingResult.Key == true && !string.IsNullOrEmpty(imageSavingResult.Value))
            {
                serviceMember.LogoUrl = imageSavingResult.Value;
            }
            else
            {
                result.Message = imageSavingResult.Value;
                return result;
            }

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

            result.Succeeded = true;
            return result;
        }

        public async Task<List<ServiceMemberForAdminModel>> GetServiceMembersForAdminAsync(string searchText)
        {
            var query = _db.ServiceMembers
                .Include(i => i.ServiceMemberBranches)
                .Include(i => i.BankAccount)
                   .ThenInclude(i => i.Bank)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchText))
            {
                query = query.Where(i => i.Name.Contains(searchText));
            }

            return await query.Select(i => new ServiceMemberForAdminModel
            {
                Id = i.Id,
                Name = i.Name,
                Email = i.Email,
                Facebook = i.FacebookUrl,
                ViewsCount = i.ViewsCount,
                Instagram = i.InstagramUrl,
                OrdersCount = i.OrdersCount,
                BranchesCount = i.ServiceMemberBranches.Count,
                AcceptsOnlinePayment = i.AcceptsOnlinePayment,
                BankAccount = i.BankAccount.ToDisplayFormat()
            }).ToListAsync();
        }
    }
}