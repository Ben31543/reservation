using Microsoft.EntityFrameworkCore;
using Reservation.Data;
using Reservation.Data.Entities;
using Reservation.Models.Common;
using Reservation.Models.Dish;
using Reservation.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reservation.Service.Services
{
    public class DishService : IDishService
    {
        private readonly ApplicationContext _db;

        public DishService(ApplicationContext db)
        {
            _db = db;
        }
        public async Task<Dish> GetDishById(long id)
        {
            return await _db.Dishes.FirstOrDefaultAsync(i => i.Id == id);
        }
        public async Task<RequestResult> AddNewDishAsync(DishModel dish)
        {
            RequestResult result = new RequestResult();

            var newDish = new Dish
            {
                Name = dish.Name,
                ImageUrl = dish.ImageUrl,
                Description = dish.Description,
                IsAvailable = dish.IsAvailable,
                DishType = dish.DishType,
                Price = dish.Price
            };

            try
            {
                await _db.Dishes.AddAsync(newDish);
                await _db.SaveChangesAsync();
                result.Succeeded = true;
            }
            catch (Exception e)
            {
                result.Message = e.Message;
            }
            result.Value = newDish;
            return result;
        }

        public async Task<RequestResult> DeleteDishAsync(long id)
        {
            RequestResult result = new RequestResult();

            var getDish = await _db.Dishes.FirstOrDefaultAsync(i => i.Id == id);
            if (getDish == null)
            {
                result.Message = "DishNotFound";
                return result;
            }

            try
            {
                _db.Dishes.Remove(getDish);
                result.Succeeded = true;
            }
            catch (Exception e)
            {
                result.Message = e.Message;
            }
            return result;
        }

        public async Task<RequestResult> EditDishAsync(DishModel dish)
        {
            RequestResult result = new RequestResult();

            var getDish = await GetDishById(dish.Id);
            if (getDish == null)
            {
                result.Message = "DishNotFound";
                result.Value = dish.Id;
                return result;
            }

            getDish.Name = dish.Name;
            getDish.Price = dish.Price;
            getDish.Description = dish.Description;
            getDish.ImageUrl = dish.ImageUrl;
            getDish.IsAvailable = dish.IsAvailable;
            getDish.DishType = dish.DishType;

            try
            {
                await _db.SaveChangesAsync();
                result.Succeeded = true;
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                return result;
            }
            result.Value = dish;
            return result;
        }

        public async Task<List<Dish>> GetAllDishAsync(long serviceMemberId)
        {
            var dishes = await _db.Dishes.Where(i => i.ServiceMemberId == serviceMemberId).ToListAsync();
            return dishes;
        }
    }
}
