using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace LinkUp_REST_API.Models
{
    public class Education
    {
        [Key]
        public Guid EducationId { get; set; }
        public string? NameOfEducation { get; set; }
        public string? Institution { get; set; }
        public int? GraduationYear { get; set; }

        // Foreign keys
        public Guid KeywordId { get; set; }

        // Navigation properties
        [JsonIgnore]
        public Keyword? Keyword { get; set; }
    }
}
