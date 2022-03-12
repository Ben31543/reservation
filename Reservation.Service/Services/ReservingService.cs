using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
		private readonly IPaymentService _paymentService;
		private readonly ILogger _logger;

		public ReservingService(ApplicationContext db, IPaymentService payment, ILogger<ReservingService> logger)
		{
			_db = db;
			_paymentService = payment;
			_logger = logger;
		}

		public async Task<RequestResult> AddReservingAsync(ReservingModel model)
		{
			RequestResult result = new RequestResult();
			var reservation = new Reserving
			{
				IsOnlinePayment = model.IsOnlinePayment,
				IsTakeOut = model.IsTakeOut,
				MemberId = model.MemberId,
				ReservationDate = model.ReservationDate,
				ServiceMemberId = model.ServiceMemberId,
				ServiceMemberBranchId = model.ServiceMemberBranchId,
				Tables = JsonConvert.SerializeObject(model.Tables),
				Dishes = JsonConvert.SerializeObject(model.Dishes),
				Notes = model.Notes,
				Amount = model.Amount
			};

			await _db.Reservings.AddAsync(reservation);

			try
			{
				await _db.SaveChangesAsync();
				result.Succeeded = true;
			}
			catch (Exception e)
			{
				result.Message = e.Message;
				_logger.LogError(e.Message);
				return result;
			}

			if (reservation.IsOnlinePayment)
			{
				await AddPaymentRequestAsync(reservation);
			}

			result.Value = reservation;
			return result;
		}

		private async Task AddPaymentRequestAsync(Reserving reserving)
		{
			var reserveData = await _db.Reservings
									   .Include(i => i.Member)
									   .Include(i => i.ServiceMember)
									   .Include(i => i.ServiceMemberBranch)
									   .FirstOrDefaultAsync(i => i.Id == reserving.Id);

			if (reserveData == null)
			{
				return;
			}

			var paymentData = new PaymentDataModel
			{
				Amount = reserving.Amount,
				PaymentDate = DateTime.Now,
				BankCardAccountFrom = reserving.Member.BankCard.Number,
				BankAcountTo = reserving.ServiceMember.BankAccount.AccountNumber
			};

			await _paymentService.AddPaymentAsync(paymentData);
		}
	}
}
