namespace Reservation.Models.ServiceMember
{
    public class ServiceMemberViewModel
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string FacebookUrl { get; set; }

        public string InstagramUrl { get; set; }

        public int OrdersCount { get; set; }

        public int ViewsCount { get; set; }

        public string LogoUrl { get; set; }

        public bool AcceptsOnlinePayment { get; set; }

        public long? BankAccountId { get; set; }
    }
}