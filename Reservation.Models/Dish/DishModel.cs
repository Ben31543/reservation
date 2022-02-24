using Reservation.Data.Enumerations;
using System.ComponentModel.DataAnnotations;

namespace Reservation.Models.Dish
{
    public class DishModel
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "FieldIsRequired")]
        [StringLength(20)]
        public string Name { get; set; }

        [Required(ErrorMessage = "FieldIsRequired")]
        public bool IsAvailable { get; set; }

        public string Description { get; set; }

        [Required(ErrorMessage = "FieldIsRequired")]
        [StringLength(250)]
        public string ImageUrl { get; set; }

        [Required(ErrorMessage = "FieldIsRequired")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "FieldIsRequired")]
        public DishesType DishType { get; set; }

    }
}
