using Reservation.Data.Entities;
using Reservation.Models.BankAccount;
using Reservation.Models.Common;
using Reservation.Models.Member;
using Reservation.Models.ServiceMember;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reservation.Service.Interfaces
{
    public interface IServiceMemberService
    {
        Task<RequestResult> RegisterServiceMemberAsync(ServiceMemberRegistrationModel model);

        Task<RequestResult> UpdateServiceMemberInfoAsync(ServiceMemberEditModel model);

        Task<RequestResult> VerifyServiceMemberAsync(MemberSignInModel model);

        Task<RequestResult> ResetPasswordAsync(MemberResetPasswordModel model);

        Task<ServiceMember> GetServiceMemberByIdAsync(long id);

        Task<RequestResult> AddBankAccountAsync(BankAccountAttachModel model);

        Task<RequestResult> DetachBankAccountAsync(long serviceMemberId, long bankAccountId);
    }
}
