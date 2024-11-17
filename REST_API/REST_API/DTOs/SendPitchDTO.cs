namespace REST_API.DTOs
{
    public class SendPitchDTO
    {
        public Guid ProfileId { get; set; }
        public String? ProfilePicture { get; set; }
        public String Name { get; set; }
        public String Title { get; set; }
        public object TextMessage { get; set; }
        public DateTime SendingDate { get; set; }
    }
}
