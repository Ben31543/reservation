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
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using Reservation.Models.Reserving;

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

            var isEmailAlreadyUsed = await _db.Members.AnyAsync(i => i.Email == model.Email);
            if (isEmailAlreadyUsed)
            {
                result.Message = LocalizationKeys.Errors.EmailAlreadyUsed;
                return result;
            }
            
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
        
        public async Task<MemberViewModel> GetMemberForViewAsync(long id)
        {
            var member = await _db.Members.FirstOrDefaultAsync(i => i.Id == id);
            if (member == null)
            {
                return null;
            }
            
            return new MemberViewModel
            {
                Id = member.Id,
                Name = member.Name,
                Surname = member.Surname,
                Email = member.Email,
                Phone = member.Phone,
            };
        }

        public async Task<string> GetBankCardNumberAsync(long memberId)
        {
            var membersBankCardId = await _db.Members
                .Where(i => i.Id == memberId)
                .Select(i => i.BankCardId)
                .FirstOrDefaultAsync();
            
            if (membersBankCardId == null)
            {
                return null;
            }

            var bankCard = await _bankCard.GetBankCardByIdAsync(membersBankCardId.Value);
            return bankCard == null
                ? null
                : bankCard.Number;
        }

        public async Task<RequestResult> ResetPasswordAsync(ResetPasswordModel member)
        {
            var result = new RequestResult();

            var existingMember = await _db.Members.FirstOrDefaultAsync(i => i.Email == member.Login);
            if (existingMember == null)
            {
                result.Message = LocalizationKeys.Errors.MemberDoesNotExist;
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
            return result;
        }

        public async Task<RequestResult> UpdateMemberInfoAsync(MemberEditModel member)
        {
            var result = new RequestResult();

            var existingMember = await GetMemberByIdAsync(member.Id);
            if (existingMember == null)
            {
                result.Message = LocalizationKeys.Errors.MemberDoesNotExist;
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

            return result;
        }

        public async Task<RequestResult> VerifyMemberAsync(SignInModel member)
        {
            var result = new RequestResult();

            var existingMember = await _db.Members.FirstOrDefaultAsync(i =>
                i.Email == member.Login
             && i.PasswordHash == member.Password.ToHashedPassword());
            
            if (existingMember == null)
            {
                result.Message = LocalizationKeys.Errors.MemberDoesNotExist;
                result.Value = member;
                return result;
            }

            result.Value = existingMember.Id;
            result.Succeeded = true;
            return result;
        }

        public async Task<RequestResult> AddBankCardAsync(AttachCardToMemberModel model)
        {
            RequestResult result = new RequestResult();
            var member = await GetMemberByIdAsync(model.MemberId);
            if (member == null)
            {
                result.Message = LocalizationKeys.Errors.MemberDoesNotExist;
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

        public async Task<RequestResult> DetachBankCardAsync(long memberId, string bankCardNumber)
        {
            var result = new RequestResult();
            var member = await GetMemberByIdAsync(memberId);
            if (member == null)
            {
                result.Message = LocalizationKeys.Errors.MemberDoesNotExist;
                return result;
            }

            var bankCardExists = await _bankCard.CheckBankCardExistsByCardNumberAsync(bankCardNumber);
            if (!bankCardExists)
            {
                result.Message = LocalizationKeys.Errors.BankCardDoesNotExist;
                return result;
            }

            try
            {
                member.BankCardId = null;
                await _db.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, e);
                result.Message = e.Message;
            }

            result.Succeeded = true;
            return result;
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
                                             ServiceMemberName = i.ServiceMember.Name,
                                             OrderedProducts = i.Dishes.ToProductsDisplayFormat(),
                                         }).ToListAsync();
        }

        public async Task<List<MemberForAdminModel>> GetMembersForAdminAsync()
        {
            return await _db.Members
                .Include(i => i.BankCard)
                .ThenInclude(i => i.Bank)
                .Select(member => new MemberForAdminModel
                {
                    Id = member.Id,
                    FullName = $"{member.Name} {member.Surname}",
                    Email = member.Email,
                    Phone = member.Phone,
                    BankCardInfo = member.BankCard.ToDisplayFormat(),
                    DealsCount = _db.Reservings
                        .Where(i => i.IsActive && i.MemberId == member.Id)
                        .Select(i => i.Id)
                        .Count()
                }).ToListAsync();
        }

        public async Task<List<MemberReservingForAdminModel>> GetMemberReservingsForAdminAsync(long memberId)
        {
            return await _db.Reservings
                .Include(i => i.ServiceMember)
                .Include(i => i.ServiceMemberBranch)
                .Where(i => i.MemberId == memberId)
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

        public async Task<Member> GetMemberByEmailAsync(string email)
        {
            return await _db.Members.FirstOrDefaultAsync(i => i.Email.Equals(email));
        }
    }
}
