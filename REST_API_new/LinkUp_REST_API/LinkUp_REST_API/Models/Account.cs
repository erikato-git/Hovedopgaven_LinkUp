using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

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
        [JsonIgnore]
        public required PersonInformation PersonInformation { get; set; }
        [JsonIgnore]
        public List<Profile>? Profiles { get; set; } = new List<Profile>();
    }
}
