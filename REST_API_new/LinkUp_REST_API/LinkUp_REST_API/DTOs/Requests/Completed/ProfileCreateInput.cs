using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LinkUp_REST_API.DTOs.Requests.Completed
{
    public class ProfileCreateInput
    {
        [StringLength(100)]
        [DefaultValue("Software Developer")]
        public required string Profession { get; set; }

        [StringLength(100)]
        [DefaultValue("Mr.")]
        public required string Title { get; set; }

        [StringLength(100)]
        [DefaultValue("")]
        public string? AlternativeTitle { get; set; }

        [StringLength(5000)]
        [DefaultValue("Profile description not provided.")]
        public string? ProfileDescription { get; set; }

        [StringLength(36)]
        [DefaultValue("00000000-0000-0000-0000-000000000000")]
        public Guid AccountId { get; set; }

        [StringLength(100)]
        [DefaultValue("Available")]
        public string? Availability { get; set; }

        [Range(0, 99, ErrorMessage = "The value must be 1-2 digits.")]
        [DefaultValue(0)]
        public int? YearsOfExperience { get; set; }

        [StringLength(100)]
        [DefaultValue("Bachelor's Degree")]
        public string? NameOfEducation { get; set; }

        [StringLength(100)]
        [DefaultValue("XYZ University")]
        public string? Institution { get; set; }

        [Range(1000, 9999, ErrorMessage = "The value must be 4 digits long")]
        [DefaultValue(2022)]
        public int? GraduationYear { get; set; }

        // Portfolio
        //public Guid? PortfolioId { get; set; }
        //public Portfolio? Portfolio { get; set; }

        // AudienceSpecification
        //public Guid? AudienceSpecificationId { get; set; }
        //public AudienceSpecification? AudienceSpecification { get; set; }
    }
}
