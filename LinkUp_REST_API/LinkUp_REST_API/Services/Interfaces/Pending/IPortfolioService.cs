using LinkUp_REST_API.Util;

namespace LinkUp_REST_API.Services.Interfaces.Pending
{
    public interface IPortfolioService
    {
        Task<ResultDTO> CreatePortfolio(object createDto, string userAccountId);
        Task<ResultDTO> GetPortfolioById(Guid id, string userAccountId);
        Task<ResultDTO> UpdatePortfolio(object updateDto, string userAccountId);
        Task<ResultDTO> DeletePortfolioById(Guid id, string userAccountId);
    }
}
