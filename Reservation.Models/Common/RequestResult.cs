namespace Reservation.Models.Common
{
    public class RequestResult
    {
        public string Message { get; set; }

        public object Value { get; set; }

        public bool Succeeded { get; set; }
    }
}
