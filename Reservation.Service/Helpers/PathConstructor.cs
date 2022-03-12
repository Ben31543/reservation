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
					return $@"\ServiceMembers\{id}\Logo\";

				case ResourceTypes.ServiceMemberImage:
					return $@"\ServiceMembers\{id}\Images\";

				case ResourceTypes.MemberImage:
					return $@"\Members\{id}\Images\";

				case ResourceTypes.ProductImage:
					return $@"\Products\{id}\";

				default:
					return string.Empty;
			}
		}
	}
}
