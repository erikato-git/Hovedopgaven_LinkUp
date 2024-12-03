namespace LinkUp_REST_API.Models.Pending
{
    public class Portfolio
    {
        public Guid PortfolioId { get; set; }
        public Guid ProfileId { get; set; }
        public Profile? Profile { get; set; }
    }
}
