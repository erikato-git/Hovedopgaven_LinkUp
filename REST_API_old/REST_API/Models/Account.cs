using System.ComponentModel.DataAnnotations;

namespace REST_API.Models
{
    public class Account
    {
        [Key]
        public Guid AccountId { get; set; }
        public String Email { get; set; }
        public String Password { get; set; }
        public List<Guid>? SavedProfileIds { get; set; } = new List<Guid>();


        // Navigation properties to PersonInformation
        public Guid PersonInformationId { get; set; }
        public PersonInformation PersonInformation { get; set; }

        // Navigation properties to Profile
        public List<Profile>? Profiles { get; set; } = new List<Profile>();
    }
}
