using LinkUp_REST_API.Models.Pending;
using System.ComponentModel.DataAnnotations;

namespace LinkUp_REST_API.Models
{
    public class Profile
    {
        [Key]
        public Guid ProfileId { get; set; }
        public required string Profession { get; set; }
        public required string Title { get; set; }
        public string? AlternativeTitle { get; set; }
        public string? ProfilePicture { get; set; }
        public string? ProfileDescription { get; set; }


        // Navigation properties to Account
        public Guid AccountId { get; set; }
        public Account? Account { get; set; }

        // Navigation properties to Keyword
        public Guid? KeywordId { get; set; }
        public Keyword? Keyword { get; set; }

        // Navigation properties to Portfolio
        public Guid? PortfolioId { get; set; }
        public Portfolio? Portfolio { get; set; }

        // Navigation properties to AudienceSpecificationId
        public Guid? AudienceSpecificationId { get; set; }
        public AudienceSpecification? AudienceSpecification { get; set; }

        // Navigation properties to Pitch
        public List<Pitch>? Pitches { get; set; }
    }
}
