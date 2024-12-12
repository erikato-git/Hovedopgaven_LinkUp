using LinkUp_REST_API.Models.Pending;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace LinkUp_REST_API.Models
{
    public class Profile
    {
        [Key]
        public Guid ProfileId { get; set; }
        public required string Profession { get; set; }
        public required string Title { get; set; }
        public string? AlternativeTitle { get; set; }
        public string? ProfileDescription { get; set; }


        // Foreign keys
        public Guid AccountId { get; set; }
        public string? MediaId { get; set; }
        public Guid? KeywordId { get; set; }
        public Guid? PortfolioId { get; set; }
        public Guid? AudienceSpecificationId { get; set; }

        // Navigation properties
        [JsonIgnore]
        public Media? ProfilePicture { get; set; }

        [JsonIgnore]
        public Account? Account { get; set; }

        [JsonIgnore]
        public Keyword? Keyword { get; set; }
        [JsonIgnore]
        public List<Pitch>? Pitches { get; set; }
        public Portfolio? Portfolio { get; set; }
        public AudienceSpecification? AudienceSpecification { get; set; }
    }
}
