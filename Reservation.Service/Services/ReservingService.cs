using Newtonsoft.Json;
using Reservation.Data;
using Reservation.Data.Entities;
using Reservation.Models.Common;
using Reservation.Models.Reserving;
using Reservation.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Web.Helpers;

namespace Reservation.Service.Services
{
    public class ReservingService : IReservingService
    {
        private readonly ApplicationContext _db;
        public ReservingService(ApplicationContext db)
        {
            _db = db;
        }
        public async Task<RequestResult> ReserveAsync(ReservingModel model)
        {
            RequestResult result = new RequestResult();
            var reserve = new Reserving
            {
                IsOnlinePayment = model.IsOnlinePayment,
                IsTakeOut = model.IsTakeOut,
                MemberId = model.MemberId,
                ReservationDate = model.ReservationDate,
                ServiceMemberId = model.ServiceMemberId,
                ServiceMemberBranchId = model.ServiceMemberBranchId,
                Tables = JsonConvert.SerializeObject(model.Tables),
                Dishes = JsonConvert.SerializeObject(model.Dishes),
                Notes = model.Notes
            };

            await _db.Reservings.AddAsync(reserve);

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

            result.Value = reserve;
            return result;
        }
    }
}
