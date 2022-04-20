using Microsoft.AspNetCore.Http;

namespace Reservation.Models.Common
{
    public class SaveImageClientModel
    {
        public IFormFile File { get; set; }

        public string ImagePath { get; set; }

        public string ResourceHost { get; set; }
    }
}