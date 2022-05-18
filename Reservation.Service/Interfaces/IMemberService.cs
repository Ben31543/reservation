using Reservation.Data.Entities;
using Reservation.Models.BankCard;
using Reservation.Models.Common;
using Reservation.Models.Member;
using System.Collections.Generic;
using System.Threading.Tasks;
using Reservation.Models.Reserving;

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

        Task<RequestResult> DetachBankCardAsync(long memberId, string bankCardNumber);

        Task<List<MemberDealsModel>> GetMemberDealsHistoryAsync(long memberId);

        Task<List<MemberForAdminModel>> GetMembersForAdminAsync();

        Task<List<MemberReservingForAdminModel>> GetMemberReservingsForAdminAsync(long memberId);

        Task<Member> GetMemberByEmailAsync(string email);

        Task<MemberViewModel> GetMemberForViewAsync(long id);

        Task<string> GetBankCardNumberAsync(long memberId);
    }
}
