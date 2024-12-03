using System.ComponentModel.DataAnnotations;

namespace LinkUp_REST_API.Models
{
    public class Account
    {
        [Key]
        public Guid AccountId { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public List<Guid>? FavoriteProfiles { get; set; } = new List<Guid>();


        // Navigation properties to PersonInformation
        public Guid PersonInformationId { get; set; }
        public required PersonInformation PersonInformation { get; set; }

        // Navigation properties to Profile
        public List<Profile>? Profiles { get; set; } = new List<Profile>();
    }
}
