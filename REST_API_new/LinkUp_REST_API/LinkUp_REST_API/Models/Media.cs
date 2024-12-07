using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace LinkUp_REST_API.Models
{
    public class Media
    {
        [Key]
        public Guid MediaId { get; set; }
        public required string URL { get; set; }

        // Foreign keys
        public Guid ProfileId { get; set; }

        // Navigation Property
        [JsonIgnore]
        public Profile? Profile { get; set; }
    }
}
