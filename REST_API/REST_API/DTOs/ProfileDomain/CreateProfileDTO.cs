namespace REST_API.DTOs.ProfileDomain
{
    public class CreateProfileDTO
    {
        public Guid ProfileId { get; set; }
        public string Profession { get; set; }
        public string Title { get; set; }
        public string? AlternativeTitle { get; set; }
        public string? ProfilePicture { get; set; }
        public string? ProfileDescription { get; set; }

        public Guid AccountId { get; set; }
        public Guid? KeywordId { get; set; }
        public Guid? PortfolioId { get; set; }
        public Guid? AudienceSpecificationId { get; set; }
        public List<Guid>? PitchIds { get; set; }
    }
}
