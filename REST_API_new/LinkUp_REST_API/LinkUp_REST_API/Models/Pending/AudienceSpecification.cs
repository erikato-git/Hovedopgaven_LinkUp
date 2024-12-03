namespace LinkUp_REST_API.Models.Pending
{
    public class AudienceSpecification
    {
        public Guid AudienceSpecificationId { get; set; }

        public Guid ProfileId { get; set; }
        public Profile? Profile { get; set; }
    }
}
