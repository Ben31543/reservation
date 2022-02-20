using Reservation.Data.Entities;
using Reservation.Models.Common;
using Reservation.Models.Member;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reservation.Service.Interfaces
{
    public interface IMemberService
    {
        Task<RequestResult> AddNewMemberAsync(MemberRegistrationModel member);

        Task<RequestResult> UpdateMemberInfoAsync(MemberEditModel member);

        Task<RequestResult> VerifyMemberAsync(MemberSignInModel member);

        Task<RequestResult> ResetPasswordAsync(MemberResetPasswordModel member);

        Task<Member> GetMemberByIdAsync(long id);
    }
}
