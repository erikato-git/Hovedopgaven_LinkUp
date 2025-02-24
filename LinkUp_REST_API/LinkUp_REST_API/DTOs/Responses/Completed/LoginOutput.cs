using LinkUp_REST_API.Models;

namespace LinkUp_REST_API.DTOs.Responses.Completed
{
    public class LoginOutput
    {
        public Guid AccountId { get; set; }
        public required string Email { get; set; }
        public required string FirstName { get; set; }
        public string? Surname { get; set; }
        public string? Phone { get; set; }
        public DateOnly BirthDate { get; set; }
        public string? Gender { get; set; }
        public required string JWT { get; set; }
        public List<Profile>? Profiles { get; set; }
    }
}
