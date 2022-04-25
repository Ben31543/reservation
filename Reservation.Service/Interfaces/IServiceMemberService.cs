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

        Task<ServiceMember> GetServiceMemberByIdAsync(long id, bool isMember = false);

        Task<RequestResult> AddBankAccountAsync(BankAccountAttachModel model);

        Task<RequestResult> DetachBankAccountAsync(long serviceMemberId, long bankAccountId);

        Task<List<ServiceMemberDealHistoryItemModel>> GetServiceMemberDealsHistoryAsync(long serviceMemberId);

        Task<List<ServiceMember>> GetServiceMembersAsync(ServiceMemberSearchCriteria criteria);

        Task<RequestResult> SaveServiceMemberImageAsync(SaveImageModel model);

        Task<List<ServiceMemberForAdminModel>> GetServiceMembersForAdminAsync(ServiceMemberSearchCriteria criteria);
    }
}
