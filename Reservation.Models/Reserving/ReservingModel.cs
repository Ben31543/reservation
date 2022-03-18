using Reservation.Data.Enumerations;
using System;
using System.Collections.Generic;

namespace Reservation.Models.Reserving
{
	public class ReservingModel
	{
		public long Id { get; set; }

		public DateTime ReservationDate { get; set; }

		public long MemberId { get; set; }

		public long ServiceMemberBranchId { get; set; }

		public long ServiceMemberId { get; set; }

		public decimal Amount { get; set; }

		public bool IsOnlinePayment { get; set; }

		public Dictionary<TableSchemas, byte> Tables { get; set; }

		public Dictionary<string, byte> Dishes { get; set; }

		public bool IsTakeOut { get; set; }

		public string Notes { get; set; }
	}
}
