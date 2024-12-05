using LinkUp_REST_API.Models;

namespace LinkUp_REST_API.DTOs.Requests
{
    public class KeywordUpdateInput
    {
        public Guid KeywordId { get; set; }
        public string? Availability { get; set; }
        public int? YearsOfExperience { get; set; }

        //public Guid ProfileId { get; set; }

        // Education
        public string? NameOfEducation { get; set; }
        public string? Institution { get; set; }
        public int? GraduationYear { get; set; }
    }
}
