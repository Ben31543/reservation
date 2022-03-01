using System.Collections.Generic;

namespace Reservation.Data.Entities
{
    public class ServiceMember
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string PasswordHash { get; set; }

        public string FacebookUrl { get; set; }

        public string InstagramUrl { get; set; }

        public int OrdersCount { get; set; }

        public string ImageUrl { get; set; }

        public string LogoUrl { get; set; }

        public bool AcceptsOnlinePayment { get; set; }

        public long? BankAccountId { get; set; }

        public BankAccount BankAccount { get; set; }

        public ICollection<ServiceMemberBranch> ServiceMemberBranches { get; set; }
    }
}
