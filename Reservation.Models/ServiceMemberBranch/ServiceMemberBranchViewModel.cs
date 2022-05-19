using Newtonsoft.Json;

namespace Reservation.Models.ServiceMemberBranch
{
    public class ServiceMemberBranchViewModel
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public long ServiceMemberId { get; set; }

        public string Phone { get; set; }

        public string WorkingHours { get; set; }

        public string TablesSchema { get; set; }

        public bool IsActive { get; set; }

        [JsonIgnore]
        public string OpenTime { get; set; }

        [JsonIgnore]
        public string CloseTime { get; set; }
    }
}