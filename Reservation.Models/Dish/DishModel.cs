using Reservation.Resources.Contents;
using System.ComponentModel.DataAnnotations;
using Reservation.Resources.Enumerations;

namespace Reservation.Models.Dish
{
    public class DishModel
    {
        public long? Id { get; set; }

        [Required(ErrorMessage = LocalizationKeys.Contents.Name + LocalizationKeys.Errors.FieldIsRequired)]
        [StringLength(20)]
        public string Name { get; set; }

        [Required(ErrorMessage = LocalizationKeys.Contents.IsAvailable + LocalizationKeys.Errors.FieldIsRequired)]
        public bool IsAvailable { get; set; }

        [Required(ErrorMessage = LocalizationKeys.Contents.Description + LocalizationKeys.Errors.FieldIsRequired)]
        public string Description { get; set; }

        [Required(ErrorMessage = LocalizationKeys.Contents.ImageUrl + LocalizationKeys.Errors.FieldIsRequired)]
        [StringLength(250)]
        public string ImageUrl { get; set; }

        [Required(ErrorMessage = LocalizationKeys.Contents.Price + LocalizationKeys.Errors.FieldIsRequired)]
        public decimal Price { get; set; }

        [Required(ErrorMessage = LocalizationKeys.Contents.Type + LocalizationKeys.Errors.FieldIsRequired)]
        public DishTypes DishType { get; set; }

        [Required(ErrorMessage = LocalizationKeys.Contents.ServiceMemberId + LocalizationKeys.Errors.FieldIsRequired)]
        public long ServiceMemberId { get; set; }
    }
}
