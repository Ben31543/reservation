using Microsoft.EntityFrameworkCore;
using Reservation.Data;
using Reservation.Data.Entities;
using Reservation.Models.Common;
using Reservation.Models.Member;
using Reservation.Service.Helpers;
using Reservation.Service.Interfaces;
using System;
using System.Threading.Tasks;

namespace Reservation.Service.Services
{
    public class MemberService : IMemberService
    {
        private readonly ApplicationContext _db;
        public MemberService(ApplicationContext db)
        {
            _db = db;
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

        public async Task<RequestResult> ResetPasswordAsync(MemberResetPasswordModel member)
        {
            var result = new RequestResult();
            var existingMember = await GetMemberByIdAsync(member.Id);
            if (existingMember == null)
            {
                result.Message = "MemberNotFound";
                result.Value = member.Id;
                return result;
            }

            if (existingMember.Email != member.LogIn || existingMember.Phone != member.LogIn)
            {
                result.Message = "WrongMember";
                result.Value = member.Id;
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
                result.Message = "MemberNotFound";
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

        public async Task<RequestResult> VerifyMemberAsync(MemberSignInModel member)
        {
            var result = new RequestResult();
            var existingMember = await _db.Members.FirstOrDefaultAsync(i =>
                i.Email == member.LogIn && i.PasswordHash == member.Password.ToHashedPassword()
             || i.Phone == member.LogIn && i.PasswordHash == member.Password.ToHashedPassword());

            if (existingMember == null)
            {
                result.Message = "MemberNotFound";
                result.Value = member;
                return result;
            }

            result.Succeeded = true;
            result.Value = existingMember;
            return result;
        }
    }
}
