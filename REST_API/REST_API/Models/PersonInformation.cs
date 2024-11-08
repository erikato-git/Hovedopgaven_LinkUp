using System.ComponentModel.DataAnnotations;

namespace REST_API.Models
{
    public class PersonInformation
    {
        [Key]
        public Guid PersonInformationId { get; set; }
        public String FirstName { get; set; }
        public String Surname { get; set; }
        public String Phone { get; set; }
        public DateOnly BirthDate { get; set; }
        public String Gender { get; set; }
    }
}
