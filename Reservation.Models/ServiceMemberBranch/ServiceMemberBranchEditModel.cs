using Reservation.Data.Enumerations;
using Reservation.Models.Common;
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

		public Time OpenTime { get; set; }

		public Time CloseTime { get; set; }

		public Dictionary<DayOfWeek, bool> WorkDays { get; set; }

		public bool? IsActive { get; set; }

		public Dictionary<TableSchemas, byte> TablesSchema { get; set; }
	}
}
