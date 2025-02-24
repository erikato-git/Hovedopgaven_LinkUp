using LinkUp_REST_API.Util.ValidationDefitions;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LinkUp_REST_API.DTOs.Requests.Completed
{
    public class ProfileDeleteInput
    {
        [DefaultValue("00000000-0000-0000-0000-000000000000")]
        public required Guid ProfileId { get; set; }

        [StrongPassword]
        [StringLength(100)]
        [DefaultValue("Password123!")]
        public required string Password { get; set; }
    }
}
