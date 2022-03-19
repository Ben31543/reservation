using Reservation.Data.Enumerations;
using Reservation.Resources.Contents;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Reservation.Models.Reserving
{
	public class ReservingModel
	{
		public long Id { get; set; }

		[Required(ErrorMessage =LocalizationKeys.Contents.ReservingDate + " " + LocalizationKeys.ErrorMessages.FieldIsRequired)]
		public DateTime ReservationDate { get; set; }

		[Required]
		public long MemberId { get; set; }

		[Required]
		public long ServiceMemberBranchId { get; set; }

		[Required]
		public long ServiceMemberId { get; set; }

		public decimal Amount { get; set; }

		[Required]
		public bool IsOnlinePayment { get; set; }

		[Required]
		public TableSchemas Tables { get; set; }

		public Dictionary<long, byte> Dishes { get; set; }

		[Required]
		public bool IsTakeOut { get; set; }

		public string Notes { get; set; }
	}
}
