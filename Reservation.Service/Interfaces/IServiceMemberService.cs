using Reservation.Data.Entities;
using Reservation.Models.BankAccount;
using Reservation.Models.Common;
using Reservation.Models.Criterias;
using Reservation.Models.ServiceMember;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Reservation.Service.Interfaces
{
    public interface IServiceMemberService
    {
        Task<RequestResult> RegisterServiceMemberAsync(ServiceMemberRegistrationModel model);

        Task<RequestResult> UpdateServiceMemberInfoAsync(ServiceMemberEditModel model);

        Task<RequestResult> VerifyServiceMemberAsync(SignInModel model);

        Task<RequestResult> ResetPasswordAsync(ResetPasswordModel model);

        Task<ServiceMember> GetServiceMemberAsync(long id);

        Task<RequestResult> AddBankAccountAsync(BankAccountAttachModel model);

        Task<RequestResult> DetachBankAccountAsync(long serviceMemberId, string accountNumber);

        Task<List<ServiceMemberDealHistoryItemModel>> GetServiceMemberDealsHistoryAsync(long serviceMemberId);

        Task<List<ServiceMember>> GetServiceMembersAsync(ServiceMemberSearchCriteria criteria);

        Task<RequestResult> SaveServiceMemberImageAsync(SaveImageModel model);

        Task<List<ServiceMemberForAdminModel>> GetServiceMembersForAdminAsync(string searchText);

        Task<ServiceMemberViewModel> GetServiceMemberByIdAsync(long smId, bool isMember = false);

        Task<string> GetServiceMembersAttachedBankAccountNumberAsync(long smId);
    }
}
