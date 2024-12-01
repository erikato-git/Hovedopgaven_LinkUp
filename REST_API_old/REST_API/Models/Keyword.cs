using System.ComponentModel.DataAnnotations;

namespace REST_API.Models
{
    public class Keyword
    {
        [Key]
        public Guid KeywordId { get; set; }
        public String? Availability { get; set; }
        public int? YearsOfExperience { get; set; }

        // Navigation properties to Profile
        public Guid ProfileId { get; set; }
        public Profile Profile { get; set; }

        // Navigation properties to Education
        public Guid? EducationId { get; set; }
        public Education? Education { get; set; }
    }
}
