using Reservation.Data.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reservation.Models.Criterias
{
    public class DishCriteria
    {
        public string SearchText { get; set; }

        public decimal? PriceMin { get; set; }

        public decimal? PriceMax { get; set; }

        public DishesType? DishType { get; set; }

        public bool? IsAvailable { get; set; }
    }
}
