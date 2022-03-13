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
					return $@"\Images\ServiceMembers\{id}\Images\";

				case ResourceTypes.MemberImage:
					return $@"\Images\Members\{id}\Images\";

				case ResourceTypes.ProductImage:
					return $@"\Images\Products\{id}\";

				default:
					return string.Empty;
			}
		}
	}
}
