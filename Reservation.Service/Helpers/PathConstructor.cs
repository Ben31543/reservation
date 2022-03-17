using Reservation.Data.Enumerations;

namespace Reservation.Service.Helpers
{
	public static class PathConstructor
	{
		public static string ConstructFilePathFor(long id, ResourceTypes resourceType)
		{
			switch (resourceType)
			{
				case ResourceTypes.ServiceMemberLogo:
					return $@"\Images\ServiceMembers\{id}\Logo\";

				case ResourceTypes.ServiceMemberImage:
					return $@"\Images\ServiceMembers\{id}\Image\";

				case ResourceTypes.MemberImage:
					return $@"\Images\Members\{id}\";

				case ResourceTypes.DishImage:
					return $@"\Images\Dishes\{id}\";

				case ResourceTypes.BranchImage:
					return $@"\Images\Branches\{id}\";

				default:
					return string.Empty;
			}
		}
	}
}
