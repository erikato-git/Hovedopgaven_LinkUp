namespace LinkUp_REST_API.DTOs.Requests.Completed
{
    public class ProfileSearchQueryInput
    {
        public string? Profession { get; set; }
        public string? Title { get; set; }
        public string? AlternativeTitle { get; set; }
        public int? Age { get; set; }
        public int? YearsOfExperience { get; set; }
        public int? GraduationYear { get; set; }
        public string? Availability { get; set; }
        public string? Institution { get; set; }
    }
}
