using Reservation.Data.Entities;
using Reservation.Models.BankCard;
using Reservation.Models.Common;
using Reservation.Models.Member;
using System.Threading.Tasks;

namespace Reservation.Service.Interfaces
{
    public interface IMemberService
    {
        Task<RequestResult> AddNewMemberAsync(MemberRegistrationModel member);

        Task<RequestResult> UpdateMemberInfoAsync(MemberEditModel member);

        Task<RequestResult> VerifyMemberAsync(SignInModel member);

        Task<RequestResult> ResetPasswordAsync(ResetPasswordModel member);

        Task<Member> GetMemberByIdAsync(long id);

        Task<RequestResult> AddBankCardAsync(AttachCardToMemberModel model);

        Task<RequestResult> DetachBankCardAsync(long memberId, long bankCardId);
    }
}
