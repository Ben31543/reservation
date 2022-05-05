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
    public static class ImageConstructorService
    {
        public static async Task<SaveImageClientModel> ConstructImageForSaveAsync(IFormFile image, string imagePath, string resourceHost = ImageSaverConstants.ImagesHostingPath)
        {
            if (image == null
                || image.Length <= 0
                || string.IsNullOrEmpty(imagePath)
                || string.IsNullOrEmpty(resourceHost))
            {
                return null;
            }
            
            string imageBase64;
            using (var ms = new MemoryStream())
            {
                await image.CopyToAsync(ms);
                var fileBytes = ms.ToArray();
                imageBase64 = Convert.ToBase64String(fileBytes);
            }

            if (!imageBase64.IsBase64())
            {
                return null;
            }

            return new SaveImageClientModel
            {
                FileName = image.FileName,
                ImageBase64 = imageBase64,
                ImagePath = imagePath,
                ResourceHost = resourceHost,
            };
        }
        
        public static string ConstructFilePathFor(ResourceTypes resourceType, long id)
        {
            switch (resourceType)
            {
                case ResourceTypes.ServiceMemberLogo:
                    return $"/Images/ServiceMembers/{id}/Logo/";

                case ResourceTypes.ServiceMemberImage:
                    return $"/Images/ServiceMembers/{id}/Image/";
                
                case ResourceTypes.MemberImage: 
                    return $"/Images/Members/{id}/";
                
                case ResourceTypes.DishImage:
                    return $"/Images/ServiceMembers/{id}/Dishes/";

                case ResourceTypes.BranchImage:
                    return $"/Images/ServiceMembers/{id}/Branches/";

                default:
                    return string.Empty;
            }
        }
    }
}