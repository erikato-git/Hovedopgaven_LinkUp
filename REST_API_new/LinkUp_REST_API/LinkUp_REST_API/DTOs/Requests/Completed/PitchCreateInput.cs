namespace LinkUp_REST_API.DTOs.Requests.Completed
{
    public class PitchCreateInput
    {
        public DateTime SendingDate { get; set; } = DateTime.Now;
        public required string TextMessage { get; set; }
        public Guid RecipientProfileId { get; set; }
        public Guid SenderProfileId { get; set; }
    }
}
