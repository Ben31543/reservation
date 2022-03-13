using Microsoft.EntityFrameworkCore;
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

namespace Reservation.Service.Services
{
    public class ServiceMemberService : IServiceMemberService
    {
        private readonly ApplicationContext _db;
        private readonly IBankAccountService _bankAccService;

        public ServiceMemberService(
            ApplicationContext db,
            IBankAccountService bankAccService)
        {
            _db = db;
            _bankAccService = bankAccService;
        }

        public async Task<ServiceMember> GetServiceMemberByIdAsync(long id)
        {
            return await _db.ServiceMembers.FirstOrDefaultAsync(i => i.Id == id);
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
                result.Message = e.Message;
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
            serviceMember.ImageUrl = model.ImageUrl;
            serviceMember.Email = model.Email;

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

            result.Value = serviceMember;
            return result;
        }

        public async Task<RequestResult> VerifyServiceMemberAsync(SignInModel model)
        {
            RequestResult result = new RequestResult();

            var serviceMember = await _db.ServiceMembers
                .FirstOrDefaultAsync(i => i.Email == model.Login && i.PasswordHash == model.Password.ToHashedPassword());
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
                result.Message = e.Message;
                return result;
            }

            return result;
        }

        public async Task<List<ServiceMemberDealHistoryItemModel>> GetServiceMemberDealsHistoryAsync(long serviceMemberId)
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
                                            OnlinePayment = i.IsOnlinePayment
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
                serviceMembers = serviceMembers.Where(i => i.AcceptsOnlinePayment == criteria.AcceptsOnlinePayment.Value);
            }

            return await serviceMembers.ToListAsync();
        }
	}
}
