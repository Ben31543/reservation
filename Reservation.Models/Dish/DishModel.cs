using Reservation.Data.Constants;
using Reservation.Data.Enumerations;
using System.ComponentModel.DataAnnotations;

namespace Reservation.Models.Dish
{
    public class DishModel
    {
        public long? Id { get; set; }

        [Required(ErrorMessage = Localizations.Errors.ThisFieldIsRequired)]
        [StringLength(20)]
        public string Name { get; set; }

        [Required(ErrorMessage = Localizations.Errors.ThisFieldIsRequired)]
        public bool IsAvailable { get; set; }

        public string Description { get; set; }

        [Required(ErrorMessage = Localizations.Errors.ThisFieldIsRequired)]
        [StringLength(250)]
        public string ImageUrl { get; set; }

        [Required(ErrorMessage = Localizations.Errors.ThisFieldIsRequired)]
        public decimal Price { get; set; }

        [Required(ErrorMessage = Localizations.Errors.ThisFieldIsRequired)]
        public DishTypes DishType { get; set; }

        [Required(ErrorMessage = Localizations.Errors.ThisFieldIsRequired)]
        public long ServiceMemberId { get; set; }
    }
}
