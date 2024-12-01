using System.ComponentModel.DataAnnotations;

namespace LinkUp_REST_API.Models
{
    public class Media
    {
        [Key]
        public Guid MediaId { get; set; }
        public string URL { get; set; }
    }
}
