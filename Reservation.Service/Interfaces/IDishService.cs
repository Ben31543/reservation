using Reservation.Data.Entities;
using Reservation.Models.Common;
using Reservation.Models.Criterias;
using Reservation.Models.Dish;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Reservation.Service.Interfaces
{
	public interface IDishService
	{
		Task<RequestResult> AddNewDishAsync(DishModel dish);

		Task<RequestResult> EditDishAsync(DishModel dish);

		Task<RequestResult> DeleteDishAsync(long id);

		Task<Dish> GetDishById(long id);

		Task<List<Dish>> GetDishesAsync(DishSearchCriteria criteria, long serviceMemberId);

		Task<RequestResult> SaveDishImageAsync(SaveImageModel model);

		Task<List<Dish>> GetAllDishAsync(long serviceMemberId);
	}
}
