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
        public Guid? ProfileId { get; set; }        // needs to be detach when profile or account are deleted, then media-element in cloudinary can be tracked an deleted

        // Navigation property
        [JsonIgnore]
        public Profile? Profile { get; set; }       
    
    }
}
