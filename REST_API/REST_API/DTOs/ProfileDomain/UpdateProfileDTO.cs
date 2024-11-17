namespace REST_API.DTOs.ProfileDomain
{
    public class UpdateProfileDTO
    {
        public Guid ProfileId { get; set; }
        public string Profession { get; set; }
        public string Title { get; set; }
        public string? AlternativeTitle { get; set; }
        public string? ProfilePicture { get; set; }
        public string? ProfileDescription { get; set; }
    }
}
