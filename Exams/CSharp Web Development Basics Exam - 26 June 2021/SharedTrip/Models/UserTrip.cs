using System.ComponentModel.DataAnnotations;

namespace SharedTrip.Models
{
    public class UserTrip
    {
        [Key]
        public string UserId { get; init; }

        public User User { get; init; }

        [Key]
        public string TripId { get; init; }

        public Trip Trip { get; init; }
    }
}
