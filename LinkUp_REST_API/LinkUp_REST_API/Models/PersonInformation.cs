using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace LinkUp_REST_API.Models
{
    public class PersonInformation
    {
        [Key]
        public Guid PersonInformationId { get; set; }
        public required string FirstName { get; set; }
        public required string Surname { get; set; }
        public string? Phone { get; set; }
        public DateOnly BirthDate { get; set; }
        public required string Gender { get; set; }          // TODO: enum

        // Foreign key
        [JsonIgnore]
        public Guid AccountId { get; set; }

    }
}
