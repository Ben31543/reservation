using System.ComponentModel.DataAnnotations;

namespace Reservation.Data.Entities
{
    public class Dish
    {
        [Key]
        public long Id { get; set; }

        public string Name { get; set; }

        public bool IsAvailable { get; set; }

        public string Description { get; set; }

        public string ImageUrl { get; set; }

        public decimal Price { get; set; }

        public byte TypeId { get; set; }

        public long ServiceMemberId { get; set; }

        public ServiceMember ServiceMember { get; set; }
    }
}
