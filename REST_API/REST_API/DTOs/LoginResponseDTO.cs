using REST_API.Models;
using System.ComponentModel.DataAnnotations;

namespace REST_API.DTOs
{
    public class LoginResponseDTO
    {
        public Account Account { get; set; }
        public String JWT { get; set; }    
    }
}
