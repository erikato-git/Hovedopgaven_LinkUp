using REST_API.Models;

namespace REST_API.DTOs
{
    public class SendPitchDTO
    {
        public DateTime SendingDate { get; set; }
        public String TextMessage { get; set; }
        public Guid? RecipientProfileId { get; set; }
        public Guid RecipientAccountId { get; set; }    // Recipient is still able to see received messages, even though recipient's profile has been deleted in the meantime

        // Navigatio property
        public Guid ProfileId { get; set; }
        public Profile? Profile { get; set; }
    }
}
