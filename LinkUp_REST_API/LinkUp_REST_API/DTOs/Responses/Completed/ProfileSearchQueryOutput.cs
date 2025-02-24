using LinkUp_REST_API.Models;

namespace LinkUp_REST_API.DTOs.Responses.Completed
{
    public class ProfileSearchQueryOutput
    {
        public Guid ProfileId { get; set; }
        public required string Profession { get; set; }
        public required string Title { get; set; }
        public string? AlternativeTitle { get; set; }
        public int? Age { get; set; }
        public int? YearsOfExperience { get; set; }
        public int? GraduationYear { get; set; }
        public string? Availability { get; set; }
        public string? Institution { get; set; }
        public Media? ProfilePicture { get; set; }
    }
}
