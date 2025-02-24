using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LinkUp_REST_API.DTOs.Requests.Completed
{
    public class ProfileSearchQueryInput
    {
        [StringLength(100)]
        [DefaultValue("Software Developer")]
        public string? Profession { get; set; }

        [StringLength(100)]
        [DefaultValue("Mr.")]
        public string? Title { get; set; }

        [StringLength(100)]
        [DefaultValue("")]
        public string? AlternativeTitle { get; set; }

        [Range(0, 999, ErrorMessage = "The value must be 1-3 digits.")]
        [DefaultValue(25)]
        public int? Age { get; set; }

        [Range(0, 99, ErrorMessage = "The value must be 1-2 digits.")]
        [DefaultValue(5)]
        public int? YearsOfExperience { get; set; }

        [Range(1000, 9999, ErrorMessage = "The value must be 4 digits long")]
        [DefaultValue(2020)]
        public int? GraduationYear { get; set; }

        [StringLength(100)]
        [DefaultValue("Available")]
        public string? Availability { get; set; }

        [StringLength(100)]
        [DefaultValue("XYZ University")]
        public string? Institution { get; set; }
    }
}
