using LinkUp_REST_API.Models;

namespace LinkUp_REST_API.DTOs.Requests
{
    public class KeywordCreateUpdateInput
    {
        public string? Availability { get; set; }
        public int? YearsOfExperience { get; set; }

        public Guid ProfileId { get; set; }

        public Guid? EducationId { get; set; }
        public Education? Education { get; set; }

    }
}
