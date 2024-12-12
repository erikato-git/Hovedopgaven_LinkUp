using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace LinkUp_REST_API.Models
{
    public class Keyword
    {
        [Key]
        public Guid KeywordId { get; set; }
        public string? Availability { get; set; }           // TODO: Enum 
        public int? YearsOfExperience { get; set; }

        // Foreign keys
        public Guid ProfileId { get; set; }
        public Guid? EducationId { get; set; }

        // Navigation properties
        [JsonIgnore]
        public Profile? Profile { get; set; }
        [JsonIgnore]
        public Education? Education { get; set; }
    }
}
