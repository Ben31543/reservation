using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Reservation.Models.Common;
using Reservation.Resources;
using Reservation.Resources.Constants;
using Reservation.Resources.Enumerations;

namespace Reservation.Service.Helpers
{
    public static class ImageHelper
    {
        public static string ConstructFilePathFor(ResourceTypes resourceType, long id)
        {
            switch (resourceType)
            {
                case ResourceTypes.ServiceMemberImage:
                    return $"/Images/ServiceMembers/{id}/Image/";
                
                case ResourceTypes.DishImage:
                    return $"/Images/ServiceMembers/{id}/Dishes/";

                default:
                    return string.Empty;
            }
        }
    }
}