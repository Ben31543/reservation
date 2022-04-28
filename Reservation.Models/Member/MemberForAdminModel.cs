namespace Reservation.Models.Member
{
    public class MemberForAdminModel
    {
        public long Id { get; set; }
        
        public string FullName { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public string BankCardInfo { get; set; }

        public int DealsCount { get; set; }
    }
}