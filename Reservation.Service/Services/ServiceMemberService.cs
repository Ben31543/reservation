using Microsoft.EntityFrameworkCore;
using Reservation.Data;
using Reservation.Data.Entities;
using Reservation.Models.Common;
using Reservation.Models.Member;
using Reservation.Models.ServiceMember;
using Reservation.Service.Helpers;
using Reservation.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reservation.Service.Services
{
    public class ServiceMemberService : IServiceMemberService
    {
        private readonly ApplicationContext _db;

        public ServiceMemberService(ApplicationContext db)
        {
            _db = db;
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

        public Task<RequestResult> ResetPasswordAsync(MemberResetPasswordModel model)
        {
            throw new NotImplementedException();
        }

        public Task<RequestResult> UpdateServiceMemberInfoAsync(MemberEditModel model)
        {
            throw new NotImplementedException();
        }

        public async Task<RequestResult> VerifyServiceMemberAsync(MemberSignInModel model)
        {
            RequestResult result = new RequestResult();

            var serviceMember = await _db.ServiceMembers
                .FirstOrDefaultAsync(i => i.Email == model.LogIn && i.PasswordHash == model.Password.ToHashedPassword());
            if (serviceMember == null)
            {
                result.Message = "WrongEmailOrPassword";
                result.Value = model;
                return result;
            }

            result.Succeeded = true;
            return result;
        }
    }
}
