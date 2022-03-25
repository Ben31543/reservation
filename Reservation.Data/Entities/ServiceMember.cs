using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Reservation.Data.Entities
{
    public class ServiceMember
    {
        [Key]
        public long Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        [JsonIgnore]
        public string PasswordHash { get; set; }

        public string FacebookUrl { get; set; }

        public string InstagramUrl { get; set; }

        public int OrdersCount { get; set; }

        public int ViewsCount { get; set; }

        public string LogoUrl { get; set; }

        public bool AcceptsOnlinePayment { get; set; }

        public long? BankAccountId { get; set; }

        public BankAccount BankAccount { get; set; }

        public ICollection<ServiceMemberBranch> ServiceMemberBranches { get; set; }
    }
}
