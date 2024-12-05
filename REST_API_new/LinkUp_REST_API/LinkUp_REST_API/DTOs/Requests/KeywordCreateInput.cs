using LinkUp_REST_API.Models;

namespace LinkUp_REST_API.DTOs.Requests
{
    public class KeywordCreateInput
    {
        public string? Availability { get; set; }
        public int? YearsOfExperience { get; set; }

        public Guid ProfileId { get; set; }

        public Education? Education { get; set; }

    }
}
