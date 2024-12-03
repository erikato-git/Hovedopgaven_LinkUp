using LinkUp_REST_API.Core.Interfaces;
using LinkUp_REST_API.Services.Interfaces.Pending;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LinkUp_REST_API.Controllers.Pending
{
    [Route("api/[controller]")]
    [ApiController]
    public class KeywordsController : ControllerBase
    {
        private IKeywordService _keywordService;
        private IAuthentication _authentication;

        public KeywordsController(IKeywordService keywordService, IAuthentication authentication)
        {
            _keywordService = keywordService;
            _authentication = authentication;
        }


        //CreateKeyword


        //GetKeywordById


        //UpdateKeyword


        //DeleteKeywordById
    }
}
