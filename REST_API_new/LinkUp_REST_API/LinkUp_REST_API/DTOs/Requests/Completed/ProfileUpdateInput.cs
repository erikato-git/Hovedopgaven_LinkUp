using LinkUp_REST_API.Models.Pending;
using LinkUp_REST_API.Models;
using System.ComponentModel.DataAnnotations;

namespace LinkUp_REST_API.DTOs.Requests.Completed
{
    public class ProfileUpdateInput
    {
        public Guid ProfileId { get; set; }
        public string? Profession { get; set; }
        public string? Title { get; set; }
        public string? AlternativeTitle { get; set; }
        public Media? ProfilePicture { get; set; }
        public string? ProfileDescription { get; set; }


        public Guid AccountId { get; set; }

        public Guid? KeywordId { get; set; }
        public Keyword? Keyword { get; set; }

        public Guid? PortfolioId { get; set; }
        public Portfolio? Portfolio { get; set; }

        public Guid? AudienceSpecificationId { get; set; }
        public AudienceSpecification? AudienceSpecification { get; set; }

        public List<Pitch>? Pitches { get; set; }
    }
}
