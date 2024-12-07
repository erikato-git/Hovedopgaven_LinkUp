using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LinkUp_REST_API.Models
{
    public class Account
    {
        [Key]
        public Guid AccountId { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public List<Guid>? FavoriteProfiles { get; set; } = new List<Guid>();


        // Foreign keys
        public Guid PersonInformationId { get; set; }

        // Navigation properties
        public required PersonInformation PersonInformation { get; set; }
        public List<Profile>? Profiles { get; set; } = new List<Profile>();
    }
}
