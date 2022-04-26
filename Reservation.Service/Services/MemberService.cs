using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Reservation.Data;
using Reservation.Data.Entities;
using Reservation.Models.BankCard;
using Reservation.Models.Common;
using Reservation.Models.Member;
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
    public class MemberService : IMemberService
    {
        private readonly ApplicationContext _db;
        private readonly IBankCardService _bankCard;
        private readonly ILogger _logger;
        private readonly IImageSavingService _imageSavingService;

        public MemberService(
            ApplicationContext db,
            IBankCardService bankCard,
            ILogger<MemberService> logger,
            IImageSavingService imageSavingService)
        {
            _db = db;
            _bankCard = bankCard;
            _logger = logger;
            _imageSavingService = imageSavingService;
        }

        public async Task<RequestResult> AddNewMemberAsync(MemberRegistrationModel model)
        {
            var result = new RequestResult();
            var member = new Member
            {
                Name = model.Name,
                Surname = model.Surname,
                Email = model.Email,
                PasswordHash = model.Password.ToHashedPassword(),
                Phone = model.Phone
            };
            
            await _db.Members.AddAsync(member);

            try
            {
                await _db.SaveChangesAsync();
                result.Succeeded = true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                result.Message = ex.Message;
                return result;
            }

            result.Value = member;
            return result;
        }

        public async Task<Member> GetMemberByIdAsync(long id)
        {
            return await _db.Members.FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<RequestResult> ResetPasswordAsync(ResetPasswordModel member)
        {
            var result = new RequestResult();

            var existingMember = await _db.Members.FirstOrDefaultAsync(i => i.Email == member.Login);
            if (existingMember == null)
            {
                result.Message = LocalizationKeys.ErrorMessages.MemberDoesNotExist;
                return result;
            }

            existingMember.PasswordHash = member.NewPassword.ToHashedPassword();

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

            result.Value = existingMember;
            return result;
        }

        public async Task<RequestResult> UpdateMemberInfoAsync(MemberEditModel member)
        {
            var result = new RequestResult();

            var existingMember = await GetMemberByIdAsync(member.Id);
            if (existingMember == null)
            {
                result.Message = LocalizationKeys.ErrorMessages.MemberDoesNotExist;
                result.Value = member.Id;
                return result;
            }

            existingMember.Name = member.Name;
            existingMember.Surname = member.Surname;
            existingMember.Phone = member.Phone;
            existingMember.Email = member.Email;

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

            result.Value = existingMember;
            return result;
        }

        public async Task<RequestResult> VerifyMemberAsync(SignInModel member)
        {
            var result = new RequestResult();

            var existingMember = await _db.Members.FirstOrDefaultAsync(i => i.Email == member.Login && i.PasswordHash == member.Password.ToHashedPassword());
            if (existingMember == null)
            {
                result.Message = LocalizationKeys.ErrorMessages.MemberDoesNotExist;
                result.Value = member;
                return result;
            }

            result.Succeeded = true;
            result.Value = existingMember;
            return result;
        }

        public async Task<RequestResult> AddBankCardAsync(AttachCardToMemberModel model)
        {
            RequestResult result = new RequestResult();
            var member = await GetMemberByIdAsync(model.MemberId);
            if (member == null)
            {
                result.Message = LocalizationKeys.ErrorMessages.MemberDoesNotExist;
                return result;
            }

            var addResult = await _bankCard.AttachCardToMemberAsync(model);
            if (!addResult.Succeeded)
            {
                return addResult;
            }

            try
            {
                member.BankCardId = (long)addResult.Value;
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

        public async Task<RequestResult> DetachBankCardAsync(long memberId, long bankCardId)
        {
            var result = new RequestResult();
            var member = await GetMemberByIdAsync(memberId);
            if (member == null)
            {
                result.Message = LocalizationKeys.ErrorMessages.MemberDoesNotExist;
                return result;
            }

            var detachResult = await _bankCard.DetachCardFromMemberAsync(bankCardId);
            if (detachResult.Succeeded)
            {
                member.BankCardId = null;
                await _db.SaveChangesAsync();
            }

            return detachResult;
        }

        public async Task<List<MemberDealsModel>> GetMemberDealsHistoryAsync(long memberId)
        {
            var orders = new List<MemberDealsModel>();

            var member = await GetMemberByIdAsync(memberId);
            if (member == null)
            {
                return orders;
            }

            return await _db.Reservings.Include(i => i.ServiceMember)
                                         .Include(i => i.ServiceMemberBranch)
                                         .Where(i => i.MemberId == memberId)
                                         .Select(i => new MemberDealsModel
                                         {
                                             Amount = i.Amount,
                                             BranchAddress = i.ServiceMemberBranch.Address,
                                             OnlinePayment = i.IsOnlinePayment,
                                             ReservingDate = i.ReservationDate,
                                             ServiceMemberName = i.ServiceMember.Name
                                         }).ToListAsync();
        }
    }
}
