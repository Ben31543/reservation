using Reservation.Data.Entities;
using Reservation.Models.Common;
using Reservation.Models.ServiceMemberBranch;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Reservation.Service.Interfaces
{
    public interface IServiceMemberBranchService
    {
        Task<RequestResult> AddBranchAsync(ServiceMemberBranchEditModel model);

        Task<RequestResult> EditBranchInfoAsync(ServiceMemberBranchEditModel model);

        Task<RequestResult> DeleteBranchAsync(long branchId);

        Task<List<ServiceMemberBranchViewModel>> GetBranchesAsync(long serviceMemberId);

        Task<ServiceMemberBranch> GetBranchByIdAsync(long branchId);

        Task<List<ServiceMemberBranchForAdminModel>> GetServiceMemberBranchesForAdminAsync(long smId);
    }
}
