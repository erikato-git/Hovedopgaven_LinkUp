using LinkUp_REST_API.Models.Pending;
using LinkUp_REST_API.Models;

namespace LinkUp_REST_API.DTOs.Requests.Completed
{
    public class ProfileCreateInput
    {
        public required string Profession { get; set; }
        public required string Title { get; set; }
        public string? AlternativeTitle { get; set; }
        public IFormFile? ProfilePicture { get; set; }
        public string? ProfileDescription { get; set; }

        public Guid AccountId { get; set; }

        public Guid? KeywordId { get; set; }
        public Keyword? Keyword { get; set; }

        public Guid? PortfolioId { get; set; }
        public Portfolio? Portfolio { get; set; }

        public Guid? AudienceSpecificationId { get; set; }
        public AudienceSpecification? AudienceSpecification { get; set; }
    }
}
