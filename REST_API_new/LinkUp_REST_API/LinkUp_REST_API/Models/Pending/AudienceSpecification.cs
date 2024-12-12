using System.Text.Json.Serialization;

namespace LinkUp_REST_API.Models.Pending
{
    public class AudienceSpecification
    {
        public Guid AudienceSpecificationId { get; set; }

        public Guid ProfileId { get; set; }
        [JsonIgnore]
        public Profile? Profile { get; set; }
    }
}
