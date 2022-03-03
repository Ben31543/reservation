using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Reservation.Data;
using Reservation.Data.Entities;
using Reservation.Models.Common;
using Reservation.Models.ServiceMemberBranch;
using Reservation.Service.Helpers;
using Reservation.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reservation.Service.Services
{
    public class ServiceMemberBranchService : IServiceMemberBranchService
    {
        private readonly ApplicationContext _db;
        private readonly IServiceMemberService _serviceMember;
        public ServiceMemberBranchService(ApplicationContext db, IServiceMemberService serviceMember)
        {
            _db = db;
            _serviceMember = serviceMember;
        }

        public async Task<RequestResult> AddBranchAsync(ServiceMemberBranchEditModel model)
        {
            RequestResult result = new RequestResult();
            var branch = new ServiceMemberBranch
            {
                Name = model.Name,
                Address = model.Address,
                Phone = model.Phone,
                TablesSchema = JsonConvert.SerializeObject(model.TablesSchema),
                OpenTime = model.OpenTime,
                CloseTime = model.CloseTime,
                IsActive = model.IsActive.Value,
                ServiceMemberId = model.ServiceMemberId,
                WorkDays = JsonConvert.SerializeObject(model.WorkDays)
            };

            await _db.ServiceMemberBranches.AddAsync(branch);
            try
            {
                await _db.SaveChangesAsync();
                result.Succeeded = true;
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                return result;
            }

            result.Value = branch;
            return result;
        }

        public async Task<RequestResult> DeleteBranchAsync(long branchId)
        {
            RequestResult result = new RequestResult();
            var branch = await GetBranchByIdAsync(branchId);
            if (branch == null)
            {
                result.Message = ErrorMessages.BranchNotFound;
                return result;
            }

            _db.ServiceMemberBranches.Remove(branch);
            try
            {
                await _db.SaveChangesAsync();
                result.Succeeded = true;
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                return result;
            }

            return result;
        }

        public async Task<RequestResult> EditBranchInfoAsync(ServiceMemberBranchEditModel model)
        {
            RequestResult result = new RequestResult();
            var branch = await GetBranchByIdAsync(model.Id.Value);
            if (branch == null)
            {
                result.Message = ErrorMessages.BranchNotFound;
                return result;
            }

            branch.Name = model.Name;
            branch.Address = model.Address;
            branch.IsActive = model.IsActive.Value;
            branch.OpenTime = model.OpenTime;
            branch.CloseTime = model.CloseTime;
            branch.Phone = model.Phone;
            branch.TablesSchema = JsonConvert.SerializeObject(model.TablesSchema);
            branch.WorkDays = JsonConvert.SerializeObject(model.WorkDays);

            try
            {
                await _db.SaveChangesAsync();
                result.Succeeded = true;
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                return result;
            }

            result.Value = branch;
            return result;
        }

        public async Task<ServiceMemberBranch> GetBranchByIdAsync(long branchId)
        {
            return await _db.ServiceMemberBranches.FirstOrDefaultAsync(i => i.Id == branchId);
        }

        public async Task<List<ServiceMemberBranch>> GetBranchesAsync(long serviceMemberId)
        {
            return await _db.ServiceMemberBranches
                .Where(i => i.ServiceMemberId == serviceMemberId)
                .ToListAsync();
        }
    }
}
