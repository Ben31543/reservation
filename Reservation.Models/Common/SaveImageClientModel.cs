namespace Reservation.Models.Common
{
    public class SaveImageClientModel
    {
        public string ImageBase64 { get; set; }

        public string ImagePath { get; set; }

        public string ResourceHost { get; set; }

        public string FileName { get; set; }
    }
}