using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LinkUp_REST_API.DTOs.Requests.Completed
{
    public class PitchCreateInput
    {
        [DefaultValue(typeof(DateTime), "2023-12-10T00:00:00")]
        public DateTime SendingDate { get; set; } = DateTime.Now;

        [StringLength(5000)]
        [DefaultValue("Default pitch message text.")]
        public required string TextMessage { get; set; }

        [StringLength(36)]
        [DefaultValue("00000000-0000-0000-0000-000000000000")]
        public Guid RecipientProfileId { get; set; }

        [StringLength(36)]
        [DefaultValue("00000000-0000-0000-0000-000000000000")]
        public Guid SenderProfileId { get; set; }
    }
}
