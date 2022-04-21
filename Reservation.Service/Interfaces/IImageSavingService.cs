using System.Collections.Generic;
using System.Threading.Tasks;
using Reservation.Models.Common;

namespace Reservation.Service.Interfaces
{
    public interface IImageSavingService
    {
        Task<KeyValuePair<bool, string>> SaveImageAsync(SaveImageClientModel model);
    }
}