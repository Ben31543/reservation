using Reservation.Data.Enumerations;

namespace Reservation.Models.Criterias
{
    public class DishCriteria
    {
        public string SearchText { get; set; }

        public decimal? PriceMin { get; set; }

        public decimal? PriceMax { get; set; }

        public DishTypes? DishType { get; set; }

        public bool? IsAvailable { get; set; }
    }
}
