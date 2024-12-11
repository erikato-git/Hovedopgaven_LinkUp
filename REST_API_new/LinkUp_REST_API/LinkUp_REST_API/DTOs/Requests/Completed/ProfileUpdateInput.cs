using LinkUp_REST_API.Models;
using LinkUp_REST_API.Util.ValidationDefitions;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LinkUp_REST_API.DTOs.Requests.Completed
{
    public class ProfileUpdateInput
    {
        [StringLength(36)]
        [DefaultValue("00000000-0000-0000-0000-000000000000")]
        public Guid ProfileId { get; set; }

        [StringLength(100)]
        [DefaultValue("Software Developer")]
        public string? Profession { get; set; }

        [StringLength(100)]
        [DefaultValue("Mr.")]
        public string? Title { get; set; }

        [StringLength(100)]
        [DefaultValue("")]
        public string? AlternativeTitle { get; set; }

        //public Media? ProfilePicture { get; set; } // TODO: Media

        [StringLength(100)]
        [DefaultValue("No description available.")]
        public string? ProfileDescription { get; set; }

        [StrongPassword]
        [StringLength(100)]
        [DefaultValue("Password123!")]
        public required string Password { get; set; }


        // Account
        //public Guid AccountId { get; set; }


        // Keyword
        [StringLength(100)]
        [DefaultValue("Available")]
        public string? Availability { get; set; }          // TODO: Enum

        [Range(1000, 9999, ErrorMessage = "The value must be 4 digits long")]
        [DefaultValue(5)]
        public int? YearsOfExperience { get; set; }

        // Education (nested)
        [StringLength(100)]
        [DefaultValue("Bachelor's Degree")]
        public string? NameOfEducation { get; set; }

        [StringLength(100)]
        [DefaultValue("XYZ University")]
        public string? Institution { get; set; }

        [Range(0, 99, ErrorMessage = "The value must be 1-2 digits long")]
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
