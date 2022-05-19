using Reservation.Resources.Contents;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Reservation.Resources.Enumerations;

namespace Reservation.Models.Reserving
{
	public class ReservingModel
	{
		public long Id { get; set; }

		public DateTime? ReservationDate { get; set; }

		[Required]
		public long MemberId { get; set; }

		[Required]
		public long ServiceMemberBranchId { get; set; }

		[Required]
		public long ServiceMemberId { get; set; }

		public bool IsOnlinePayment { get; set; } = false;

		[Required]
		public byte TablesSchemaId { get; set; }

		public Dictionary<long, byte> Dishes { get; set; }

		public bool IsTakeOut { get; set; } = false;

		public string Notes { get; set; }
	}
}
