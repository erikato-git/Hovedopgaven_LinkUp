using System.ComponentModel.DataAnnotations;

namespace LinkUp_REST_API.Models
{
    public class Education
    {
        [Key]
        public Guid EducationId { get; set; }
        public required string NameOfEducation { get; set; }
        public required string Institution { get; set; }
        public required int GraduationYear { get; set; }

        // Navigation properties to Keyword
        public Guid KeywordId { get; set; }
        public Keyword? Keyword { get; set; }
    }
}
