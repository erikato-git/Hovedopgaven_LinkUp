using REST_API.Models;

namespace REST_API.DTOs.AccountDomain
{
    public class LoginResponseDTO
    {
        public Account Account { get; set; }
        public string JWT { get; set; }
    }
}
