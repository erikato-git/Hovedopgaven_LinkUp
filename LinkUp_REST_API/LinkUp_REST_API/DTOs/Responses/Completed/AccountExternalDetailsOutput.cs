namespace LinkUp_REST_API.DTOs.Responses.Completed
{
    public class AccountExternalDetailsOutput
    {
        public Guid AccountId { get; set; }
        public required string Email { get; set; }
        public required string FirstName { get; set; }
        public required string Surname { get; set; }
        public string? Phone { get; set; }
        public DateOnly BirthDate { get; set; }
        public required string Gender { get; set; }
    }
}
