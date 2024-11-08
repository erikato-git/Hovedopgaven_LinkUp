namespace REST_API.Models
{
    public class Portfolio
    {
        public Guid PortfolioId { get; set; }
        public List<String>? Projects { get; set; }      // TODO: not completely thought through

        // Navigation properties
        public Guid ProfileId { get; set; }
        public Profile Profile { get; set; }
    }
}
