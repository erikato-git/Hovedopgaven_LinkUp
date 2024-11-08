namespace REST_API.Models
{
    public class Pitch
    {
        public Guid PitchId { get; set; }
        public DateTime SendingDate { get; set; }
        public String TextMessage { get; set; }

        // Navigation properties
        public Guid? ProfileId { get; set; }
        public Profile? Profile { get; set; }
    }
}
