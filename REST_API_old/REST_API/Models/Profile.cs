using System.ComponentModel.DataAnnotations;

namespace REST_API.Models
{
    public class Profile
    {
        [Key]
        public Guid ProfileId { get; set; }
        public String Profession { get; set; }
        public String Title { get; set; }
        public String? AlternativeTitle { get; set; }
        public String? ProfilePicture { get; set; }
        public String? ProfileDescription { get; set; }

        // Navigation properties to Account
        public Guid AccountId { get; set; }
        public Account Account { get; set; }

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
