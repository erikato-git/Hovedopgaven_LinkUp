using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace LinkUp_REST_API.Models
{
    public class Media
    {
        [Key]
        public required string MediaId { get; set; }
        public required string URL { get; set; }

        // Foreign keys
        public Guid ProfileId { get; set; }

        // Navigation property
        [JsonIgnore]
        public Profile? Profile { get; set; }        // TODO: How can I ensure that images that belongs to a profile / account are deleted together from Cloudinary
    
    }
}
