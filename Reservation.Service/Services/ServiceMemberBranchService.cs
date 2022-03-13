using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Reservation.Data;
using Reservation.Data.Entities;
using Reservation.Models.Common;
using Reservation.Models.ServiceMemberBranch;
using Reservation.Resources.Contents;
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
        public ServiceMemberBranchService(ApplicationContext db)
        {
            _db = db;
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
                OpenTime = JsonConvert.SerializeObject(model.OpenTime),
                CloseTime = JsonConvert.SerializeObject(model.CloseTime),
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
                result.Message = LocalizationKeys.ErrorMessages.BranchNotFound;
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
                result.Message = LocalizationKeys.ErrorMessages.BranchNotFound;
                return result;
            }

            branch.Name = model.Name;
            branch.Address = model.Address;
            branch.IsActive = model.IsActive.Value;
            branch.OpenTime = JsonConvert.SerializeObject(model.OpenTime);
            branch.CloseTime = JsonConvert.SerializeObject(model.CloseTime);
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
