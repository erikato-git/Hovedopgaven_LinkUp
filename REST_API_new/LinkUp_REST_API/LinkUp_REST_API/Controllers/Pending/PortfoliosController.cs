using LinkUp_REST_API.Core.Interfaces;
using LinkUp_REST_API.Services.Interfaces.Pending;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LinkUp_REST_API.Controllers.Pending
{
    [Route("api/[controller]")]
    [ApiController]
    public class PortfoliosController : ControllerBase
    {
        private IPortfolioService _portfolioService;
        private IAuthentication _authentication;

        public PortfoliosController(IPortfolioService portfolioService, IAuthentication authentication)
        {
            _portfolioService = portfolioService;
            _authentication = authentication;
        }


        //CreatePortfolio


        //GetPortfolioById


        //UpdatePortfolio


        //DeletePortfolioById
    }
}
