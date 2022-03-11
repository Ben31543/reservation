﻿using Microsoft.EntityFrameworkCore;
using Reservation.Data;
using Reservation.Data.Entities;
using Reservation.Models.BankCard;
using Reservation.Models.Common;
using Reservation.Models.Member;
using Reservation.Service.Helpers;
using Reservation.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reservation.Service.Services
{
    public class MemberService : IMemberService
    {
        private readonly ApplicationContext _db;
        private readonly IBankCardService _bankCard;
        public MemberService(ApplicationContext db, IBankCardService bankCard)
        {
            _db = db;
            _bankCard = bankCard;
        }
        public async Task<RequestResult> AddNewMemberAsync(MemberRegistrationModel member)
        {
            var result = new RequestResult();
            var newMember = new Member
            {
                Name = member.Name,
                Surname = member.SurName,
                BirthDate = member.BirthDate,
                Email = member.Email,
                PasswordHash = member.Password.ToHashedPassword(),
                Phone = member.Phone
            };

            try
            {
                _db.Members.Add(newMember);
                await _db.SaveChangesAsync();
                result.Succeeded = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }

            result.Value = newMember;
            return result;
        }

        public async Task<Member> GetMemberByIdAsync(long id)
        {
            return await _db.Members.FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<RequestResult> ResetPasswordAsync(ResetPasswordModel member)
        {
            var result = new RequestResult();
            var existingMember = await _db.Members.FirstOrDefaultAsync(i => i.Email == member.Login || i.Phone == member.Login);
            if (existingMember == null)
            {
                result.Message = ErrorMessages.MemberDoesNotExist;
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
                result.Message = e.Message;
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
                result.Message = ErrorMessages.MemberDoesNotExist;
                result.Value = member.Id;
                return result;
            }

            existingMember.Name = member.Name;
            existingMember.Surname = member.Surname;
            existingMember.Phone = member.Phone;
            existingMember.Email = member.Email;
            existingMember.BirthDate = member.BirthDate;

            try
            {
                await _db.SaveChangesAsync();
                result.Succeeded = true;
            }
            catch (Exception e)
            {
                result.Message = e.Message;
            }

            result.Value = existingMember;
            return result;
        }

        public async Task<RequestResult> VerifyMemberAsync(SignInModel member)
        {
            var result = new RequestResult();
            var existingMember = await _db.Members.FirstOrDefaultAsync(i =>
                i.Email == member.LogIn && i.PasswordHash == member.Password.ToHashedPassword()
             || i.Phone == member.LogIn && i.PasswordHash == member.Password.ToHashedPassword());

            if (existingMember == null)
            {
                result.Message = ErrorMessages.MemberDoesNotExist;
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
                result.Message = ErrorMessages.MemberDoesNotExist;
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
                result.Message = ErrorMessages.MemberDoesNotExist;
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
            List<MemberDealsModel> orders = new List<MemberDealsModel>();

            var member = await GetMemberByIdAsync(memberId);
            if (member == null)
            {
                return orders;
            }

            orders = await _db.Reservings.Include(i => i.ServiceMember)
                                      .Include(i => i.ServiceMemberBranch)
                                      .Where(i => i.MemberId == memberId)
                                      .Select(i => new MemberDealsModel
                                      {
                                          Amount = i.Amount,
                                          Address = i.ServiceMemberBranch.Address,
                                          OnlinePayment = i.IsOnlinePayment,
                                          OrdersDate = i.ReservationDate,
                                          ServiceMemberName = i.ServiceMember.Name

                                      }).ToListAsync();

            return orders;
        }
    }
}
