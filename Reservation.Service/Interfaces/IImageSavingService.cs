using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Reservation.Service.Interfaces
{
    public interface IImageSavingService
    {
        Task<string> SaveImageAsync(IFormFile file, string resourceHost, string imagePath);
    }
}