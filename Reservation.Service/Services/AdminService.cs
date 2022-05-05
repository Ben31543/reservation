using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Reservation.Data;
using Reservation.Models.Common;
using Reservation.Resources.Contents;
using Reservation.Service.Helpers;
using Reservation.Service.Interfaces;
using System.Threading.Tasks;

namespace Reservation.Service.Services
{
    public class AdminService : IAdminService
    {
        private readonly ApplicationContext _db;
        private readonly ILogger<AdminService> _logger;

        public AdminService(ApplicationContext db, ILogger<AdminService> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<RequestResult> VerifyAdminAsync(string username, string password)
        {
            var result = new RequestResult();

            var admin = await _db.Admins.FirstOrDefaultAsync(i => i.Username == username && i.PasswordHash == password.ToHashedPassword());
            if (admin == null)
            {
                result.Message = LocalizationKeys.Errors.WrongCredientials;
                return result;
            }

            result.Succeeded = true;
            return result;
        }

        public async Task<decimal> GetReservationServiceCurrencyTurnoverAsync()
        {
            return await _db.Reservings.SumAsync(i => i.Amount);
        }
    }
}
