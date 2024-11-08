using System.ComponentModel.DataAnnotations;

namespace REST_API.Models
{
    public class Education
    {
        [Key]
        public Guid EducationId { get; set; }
        public String NameOfEducation { get; set; }
        public String Institution { get; set; }
        public String GraduationYear { get; set; }

        // Navigation properties to Keyword
        public Guid KeywordId { get; set; }
        public Keyword Keyword { get; set; }
    }
}
