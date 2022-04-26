using System.ComponentModel.DataAnnotations;

namespace Reservation.Data.Entities
{
    public class Admin
    {
        [Key]
        public long Id { get; set; }

        public string Username { get; set; }

        public string PasswordHash { get; set; }
    }
}
