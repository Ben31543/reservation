using Reservation.Models.Common;
using System.Threading.Tasks;

namespace Reservation.Service.Interfaces
{
    public interface IAdminService
    {
        Task<RequestResult> VerifyAdminAsync(string username, string password);
    }
}
