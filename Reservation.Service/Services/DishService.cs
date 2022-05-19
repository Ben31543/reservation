using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Reservation.Data;
using Reservation.Data.Entities;
using Reservation.Models.Common;
using Reservation.Models.Criterias;
using Reservation.Models.Dish;
using Reservation.Resources.Contents;
using Reservation.Service.Helpers;
using Reservation.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Reservation.Resources.Constants;
using Reservation.Resources.Enumerations;

namespace Reservation.Service.Services
{
    public class DishService : IDishService
    {
        private readonly ApplicationContext _db;
        private readonly ILogger _logger;
        private readonly IImageSavingService _imageSavingService;

        public DishService(
            ApplicationContext db,
            ILogger<DishService> logger,
            IImageSavingService imageSavingService)
        {
            _db = db;
            _logger = logger;
            _imageSavingService = imageSavingService;
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
                TypeId = (byte) dish.DishType,
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
                result.Message = LocalizationKeys.Errors.DishDoesNotExist;
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
                result.Message = LocalizationKeys.Errors.DishDoesNotExist;
                result.Value = model.Id;
                return result;
            }

            dish.Name = model.Name;
            dish.Price = model.Price;
            dish.Description = model.Description;
            dish.ImageUrl = model.ImageUrl;
            dish.IsAvailable = model.IsAvailable;
            dish.TypeId = (byte) model.DishType;

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

        public async Task<List<DishModel>> GetDishesAsync(DishSearchCriteria criteria)
        {
            var dishes = await _db.Dishes
                .Where(i => i.ServiceMemberId == criteria.ServiceMemberId)
                .ToListAsync();

            if (!dishes.Any())
            {
                return new List<DishModel>();
            }

            if (criteria.DishType is > 0)
            {
                dishes = dishes.Where(i => i.TypeId == (byte) criteria.DishType).ToList();
            }

            if (criteria.PriceMin.HasValue)
            {
                dishes = dishes.Where(i => i.Price >= criteria.PriceMin).ToList();
            }

            if (criteria.PriceMax.HasValue)
            {
                dishes = dishes.Where(i => i.Price <= criteria.PriceMax).ToList();
            }

            if (criteria.IsAvailable)
            {
                dishes = dishes.Where(i => i.IsAvailable == criteria.IsAvailable).ToList();
            }

            if (!string.IsNullOrWhiteSpace(criteria.SearchText))
            {
                dishes = dishes.Where(i => i.Name.Contains(criteria.SearchText, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            return dishes.Select(dish => new DishModel
            {
                Id = dish.Id,
                Name = dish.Name,
                Description = dish.Description,
                Price = dish.Price,
                DishType = (DishTypes) dish.TypeId,
                ImageUrl = dish.ImageUrl,
                IsAvailable = dish.IsAvailable,
                ServiceMemberId = dish.ServiceMemberId
            }).ToList();
        }

        public async Task<RequestResult> SaveDishImageAsync(SaveImageModel model)
        {
            RequestResult result = new RequestResult();

            var dish = await GetDishById(model.Id.Value);
            if (dish == null)
            {
                result.Message = LocalizationKeys.Errors.WrongIncomingParameters;
                return result;
            }

            var image = new SaveImageClientModel
            {
                ImageBase64 = model.ImageBase64,
                ImagePath = ImageHelper.ConstructFilePathFor(model.ResourceType.Value, dish.ServiceMemberId),
                ResourceHost = ImageSaverConstants.ImagesHostingPath,
                FileName = $"{model.ResourceType}{dish.ServiceMemberId}"
            };

            var imageSavingResult = await _imageSavingService.SaveImageAsync(image);
            if (imageSavingResult.Key == true && !string.IsNullOrEmpty(imageSavingResult.Value))
            {
                dish.ImageUrl = imageSavingResult.Value;    
            }
            else
            {
                result.Message = imageSavingResult.Value;
                return result;
            }
            
            try
            {
                await _db.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                result.Message = e.Message;
                return result;
            }

            result.Succeeded = true;
            return result;
        }
    }
}