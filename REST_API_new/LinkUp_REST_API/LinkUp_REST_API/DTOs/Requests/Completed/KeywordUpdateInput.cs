using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LinkUp_REST_API.DTOs.Requests.Completed
{
    public class KeywordUpdateInput
    {
        [StringLength(36)]
        [DefaultValue("00000000-0000-0000-0000-000000000000")]
        public Guid KeywordId { get; set; }

        [StringLength(100)]
        [DefaultValue("Available")]
        public string? Availability { get; set; }

        [Range(0, 99, ErrorMessage = "The value must be 2 digits")]
        [DefaultValue(0)]
        public int? YearsOfExperience { get; set; }

        // Education
        [StringLength(100)]
        [DefaultValue("Bachelor's Degree")]
        public string? NameOfEducation { get; set; }

        [StringLength(100)]
        [DefaultValue("XYZ University")]
        public string? Institution { get; set; }

        [Range(1000, 9999, ErrorMessage = "The value must be 4 digits")]
        [DefaultValue(2022)]
        public int? GraduationYear { get; set; }
    }
}
