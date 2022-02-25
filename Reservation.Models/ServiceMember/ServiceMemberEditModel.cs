using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reservation.Models.ServiceMember
{
    public class ServiceMemberEditModel
    {
        public long? Id { get; set; }

        [Required(ErrorMessage = "FieldIsRequried")]
        [StringLength(50)]
        public string Name { get; set; }

        [Required(ErrorMessage = "FieldIsRequried")]
        [StringLength(255)]
        public string Email { get; set; }

        [StringLength(255)]
        public string FacebookUrl { get; set; }

        [StringLength(255)]
        public string InstagramUrl { get; set; }

        public string ImageUrl { get; set; }

        public string LogoUrl { get; set; }

        [Required(ErrorMessage = "FieldIsRequried")]
        public bool AcceptsOnlinePayment { get; set; }
    }
}
