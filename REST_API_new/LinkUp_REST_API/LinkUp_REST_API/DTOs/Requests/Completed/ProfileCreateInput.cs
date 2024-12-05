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

        // Keyword
        public string? Availability { get; set; }           // TODO: Enum
        public int? YearsOfExperience { get; set; }
        
        // Education (nested)
        public string? NameOfEducation { get; set; }
        public string? Institution { get; set; }
        public int? GraduationYear { get; set; }

        // Portfolio
        //public Guid? PortfolioId { get; set; }
        //public Portfolio? Portfolio { get; set; }

        // AudienceSpecification
        //public Guid? AudienceSpecificationId { get; set; }
        //public AudienceSpecification? AudienceSpecification { get; set; }
    }
}
