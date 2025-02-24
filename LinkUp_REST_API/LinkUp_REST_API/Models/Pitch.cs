using System.Text.Json.Serialization;

namespace LinkUp_REST_API.Models
{
    public class Pitch
    {
        public Guid PitchId { get; set; }
        public DateTime SendingDate { get; set; }
        public required string TextMessage { get; set; }
        public Guid RecipientProfileId { get; set; }

        // Foreign keys
        public Guid ProfileId { get; set; }

        // Navigation properties
        [JsonIgnore]
        public Profile? Profile { get; set; }
    }
}
