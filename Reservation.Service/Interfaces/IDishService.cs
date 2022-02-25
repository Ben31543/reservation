﻿using Reservation.Data.Entities;
using Reservation.Models.Common;
using Reservation.Models.Dish;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reservation.Service.Interfaces
{
    public interface IDishService
    {
        Task<RequestResult> AddNewDishAsync(DishModel dish);

        Task<RequestResult> EditDishAsync(DishModel dish);

        Task<RequestResult> DeleteDishAsync(long id);

        Task<List<Dish>> GetAllDishAsync(long serviceMemberId);

        Task<Dish> GetDishById(long id);
    }
}