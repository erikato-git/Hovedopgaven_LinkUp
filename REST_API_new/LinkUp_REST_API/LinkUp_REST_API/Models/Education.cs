using System.ComponentModel.DataAnnotations;

namespace LinkUp_REST_API.Models
{
    public class Education
    {
        [Key]
        public Guid EducationId { get; set; }
        public string? NameOfEducation { get; set; }
        public string? Institution { get; set; }
        public int? GraduationYear { get; set; }

        // Navigation properties to Keyword
        public Guid KeywordId { get; set; }
        public Keyword? Keyword { get; set; }
    }
}
