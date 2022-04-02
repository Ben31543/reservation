using Reservation.Resources.Enumerations;

namespace Reservation.Service.Helpers
{
    public static class PathConstructor
    {
        public static string ConstructFilePathFor(ResourceTypes resourceType, long id)
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
                    return $@"\Images\ServiceMembers\{id}\Dishes\";

                case ResourceTypes.BranchImage:
                    return $@"\Images\ServiceMembers\{id}\Branches\";

                default:
                    return string.Empty;
            }
        }
    }
}