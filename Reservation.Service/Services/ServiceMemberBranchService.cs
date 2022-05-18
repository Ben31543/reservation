using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
using Reservation.Service.Helpers;

namespace Reservation.Service.Services
{
    public class ServiceMemberBranchService : IServiceMemberBranchService
    {
        private readonly ApplicationContext _db;
        private readonly ILogger _logger;

        public ServiceMemberBranchService(
            ApplicationContext db,
            ILogger<ServiceMemberBranchService> logger)
        {
            _db = db;
            _logger = logger;
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
            };

            await _db.ServiceMemberBranches.AddAsync(branch);
            try
            {
                await _db.SaveChangesAsync();
                result.Succeeded = true;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
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
                result.Message = LocalizationKeys.Errors.BranchNotFound;
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
                _logger.LogError(e.Message);
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
                result.Message = LocalizationKeys.Errors.BranchNotFound;
                return result;
            }

            branch.Name = model.Name;
            branch.Address = model.Address;
            branch.IsActive = model.IsActive.Value;
            branch.OpenTime = model.OpenTime.ToString();
            branch.CloseTime = model.CloseTime.ToString();
            branch.Phone = model.Phone;
            branch.TablesSchema = JsonConvert.SerializeObject(model.TablesSchema);

            try
            {
                await _db.SaveChangesAsync();
                result.Succeeded = true;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
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

        public async Task<List<ServiceMemberBranchViewModel>> GetBranchesAsync(long serviceMemberId)
        {
            return await _db.ServiceMemberBranches
                .Where(i => i.ServiceMemberId == serviceMemberId)
                .Select(branch => new ServiceMemberBranchViewModel
                {
                    Id = branch.Id,
                    Name = branch.Name,
                    Address = branch.Address,
                    Phone = branch.Phone,
                    IsActive = branch.IsActive,
                    TablesSchema = branch.TablesSchema,
                    WorkingHours = Time.ToDisplayFormat(branch.OpenTime, branch.CloseTime),
                    ServiceMemberId = branch.ServiceMemberId
                })
                .ToListAsync();
        }

        public async Task<List<ServiceMemberBranchForAdminModel>> GetServiceMemberBranchesForAdminAsync(long smId)
        {
            return await _db.ServiceMemberBranches
                .Where(i => i.ServiceMemberId == smId)
                .Select(branch => new ServiceMemberBranchForAdminModel
                {
                    ServiceMemberName = branch.ServiceMember.Name,
                    Address = branch.Address,
                    IsActive = branch.IsActive,
                    Phone = branch.Phone,
                    WorkingHours = Time.ToDisplayFormat(branch.OpenTime.ToTimeInstance(), branch.CloseTime.ToTimeInstance()),
                    OrdersCount = _db.Reservings
                                     .Where(reserving =>
                                            reserving.IsActive
                                         && reserving.ServiceMemberId == branch.ServiceMemberId
                                         && reserving.ServiceMemberBranchId == branch.Id)
                                     .Select(r => r.Id)
                                     .Count()
                }).ToListAsync();
        }
    }
}
