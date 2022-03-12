using Reservation.Data.Enumerations;
using System;
using System.Collections.Generic;

namespace Reservation.Models.ServiceMemberBranch
{
	public class ServiceMemberBranchEditModel
	{
		public long? Id { get; set; }

		public string Name { get; set; }

		public string Address { get; set; }

		public long ServiceMemberId { get; set; }

		public string Phone { get; set; }

		public TimeSpan OpenTime { get; set; }

		public TimeSpan CloseTime { get; set; }

		public Dictionary<DayOfWeek, bool> WorkDays { get; set; }

		public bool? IsActive { get; set; }

		public Dictionary<TableSchemas, byte> TablesSchema { get; set; }
	}
}
