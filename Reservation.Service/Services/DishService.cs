using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Reservation.Data;
using Reservation.Data.Entities;
using Reservation.Models.Common;
using Reservation.Models.Criterias;
using Reservation.Models.Dish;
using Reservation.Resources.Contents;
using Reservation.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reservation.Service.Services
{
    public class DishService : IDishService
    {
        private readonly ApplicationContext _db;
        private readonly ILogger _logger;

        public DishService(
            ApplicationContext db,
            ILogger<DishService> logger)
        {
            _db = db;
            _logger = logger;
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
                TypeId = (byte)dish.DishType,
                Price = dish.Price,
                ServiceMemberId = dish.ServiceMemberId
            };

            await _db.Dishes.AddAsync(newDish);

            try
            {
                await _db.SaveChangesAsync();
                result.Succeeded = true;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                result.Message = e.Message;
                return result;
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
                result.Message = LocalizationKeys.ErrorMessages.DishDoesNotExist;
                return result;
            }

            _db.Dishes.Remove(getDish);
            try
            {
                await _db.SaveChangesAsync();
                result.Succeeded = true;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                result.Message = e.Message;
                return result;
            }
            return result;
        }

        public async Task<RequestResult> EditDishAsync(DishModel model)
        {
            RequestResult result = new RequestResult();

            var dish = await GetDishById(model.Id.Value);
            if (dish == null)
            {
                result.Message = LocalizationKeys.ErrorMessages.DishDoesNotExist;
                result.Value = model.Id;
                return result;
            }

            dish.Name = model.Name;
            dish.Price = model.Price;
            dish.Description = model.Description;
            dish.ImageUrl = model.ImageUrl;
            dish.IsAvailable = model.IsAvailable;
            dish.TypeId = (byte)model.DishType;

            try
            {
                await _db.SaveChangesAsync();
                result.Succeeded = true;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                result.Message = e.Message;
                return result;
            }
            result.Value = model;
            return result;
        }

        public async Task<List<Dish>> GetAllDishAsync(long serviceMemberId)
        {
            return await _db.Dishes.Where(i => i.ServiceMemberId == serviceMemberId).ToListAsync();
        }

        public async Task<List<Dish>> GetDishesAsync(DishSearchCriteria criteria, long serviceMemberId)
        {
            var dishes = _db.Dishes
                .Where(i => i.ServiceMemberId == serviceMemberId)
                .AsNoTracking()
                .AsQueryable();

			if (!dishes.Any())
			{
                return new List<Dish>();
			}

            if (criteria.DishType.HasValue)
            {
                dishes = dishes.Where(i => i.TypeId == (byte)criteria.DishType);
            }

            if (criteria.PriceMin.HasValue)
            {
                dishes = dishes.Where(i => i.Price >= criteria.PriceMin);
            }

            if (criteria.PriceMax.HasValue)
            {
                dishes = dishes.Where(i => i.Price <= criteria.PriceMax);
            }

            if (criteria.IsAvailable.HasValue)
            {
                dishes = dishes.Where(i => i.IsAvailable == criteria.IsAvailable);
            }

            if (!string.IsNullOrWhiteSpace(criteria.SearchText))
            {
                dishes = dishes.Where(i => i.Name.Contains(criteria.SearchText,StringComparison.OrdinalIgnoreCase));
            }

            return await dishes.ToListAsync();
        }
    }
}
