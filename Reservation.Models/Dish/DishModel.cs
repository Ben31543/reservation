using Reservation.Resources.Contents;
using System.ComponentModel.DataAnnotations;
using Reservation.Resources.Enumerations;

namespace Reservation.Models.Dish
{
    public class DishModel
    {
        public long? Id { get; set; }

        [Required(ErrorMessage = LocalizationKeys.Contents.Name + LocalizationKeys.ErrorMessages.FieldIsRequired)]
        [StringLength(20)]
        public string Name { get; set; }

        [Required(ErrorMessage = LocalizationKeys.Contents.IsAvailable + LocalizationKeys.ErrorMessages.FieldIsRequired)]
        public bool IsAvailable { get; set; }

        [Required(ErrorMessage = LocalizationKeys.Contents.Description + LocalizationKeys.ErrorMessages.FieldIsRequired)]
        public string Description { get; set; }

        [Required(ErrorMessage = LocalizationKeys.Contents.ImageUrl + LocalizationKeys.ErrorMessages.FieldIsRequired)]
        [StringLength(250)]
        public string ImageUrl { get; set; }

        [Required(ErrorMessage = LocalizationKeys.Contents.Price + LocalizationKeys.ErrorMessages.FieldIsRequired)]
        public decimal Price { get; set; }

        [Required(ErrorMessage = LocalizationKeys.Contents.Type + LocalizationKeys.ErrorMessages.FieldIsRequired)]
        public DishTypes DishType { get; set; }

        [Required(ErrorMessage = LocalizationKeys.Contents.ServiceMemberId + LocalizationKeys.ErrorMessages.FieldIsRequired)]
        public long ServiceMemberId { get; set; }
    }
}
