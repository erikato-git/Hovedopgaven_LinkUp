using System.ComponentModel.DataAnnotations;

namespace LinkUp_REST_API.Models
{
    public class Keyword
    {
        [Key]
        public Guid KeywordId { get; set; }
        public string? Availability { get; set; }           // TODO: Enum
        public int? YearsOfExperience { get; set; }

        // Navigation properties to Profile
        public Guid ProfileId { get; set; }
        public Profile? Profile { get; set; }

        // Navigation properties to Education
        public Guid? EducationId { get; set; }
        public Education? Education { get; set; }
    }
}
