using Reservation.Resources.Enumerations;

namespace Reservation.Models.Criterias
{
    public class DishSearchCriteria
    {
        public long? ServiceMemberId { get; set; }

        public string SearchText { get; set; }

        public decimal? PriceMin { get; set; }

        public decimal? PriceMax { get; set; }

        public DishTypes? DishType { get; set; }

        public bool IsAvailable { get; set; }
    }
}
