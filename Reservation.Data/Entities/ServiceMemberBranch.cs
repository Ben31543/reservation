using System;
using System.ComponentModel.DataAnnotations;

namespace Reservation.Data.Entities
{
    public class ServiceMemberBranch
    {
        [Key]
        public long Id { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public long ServiceMemberId { get; set; }

        public string Phone { get; set; }

        public string OpenTime { get; set; }

        public string CloseTime { get; set; }

        public string TablesSchema { get; set; }

        public bool IsActive { get; set; }

        public ServiceMember ServiceMember { get; set; }
    }
}
