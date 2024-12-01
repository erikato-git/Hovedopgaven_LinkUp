using System.ComponentModel.DataAnnotations;

namespace REST_API.Models
{
    public class AudienceSpecification
    {
        [Key]
        public Guid AudienceSpecificationId { get; set; }
        public List<String>? Professions { get; set; }

        // Navigation properties to Profile
        public Guid ProfileId { get; set; }
        public Profile Profile { get; set; }
    }
}
