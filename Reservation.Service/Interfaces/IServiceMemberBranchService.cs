using Reservation.Data.Entities;
using Reservation.Models.Common;
using Reservation.Models.ServiceMemberBranch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reservation.Service.Interfaces
{
    public interface IServiceMemberBranchService
    {
        Task<RequestResult> AddBranchAsync(ServiceMemberBranchEditModel model);

        Task<RequestResult> EditBranchInfoAsync(ServiceMemberBranchEditModel model);

        Task<RequestResult> DeleteBranchAsync(long branchId);

        Task<List<ServiceMemberBranch>> GetBranchesAsync(long serviceMemberId);

        Task<ServiceMemberBranch> GetBranchByIdAsync(long branchId);
    }
}
