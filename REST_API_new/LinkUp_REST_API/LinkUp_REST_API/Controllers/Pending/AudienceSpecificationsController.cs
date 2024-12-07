using LinkUp_REST_API.Core.Interfaces;
using LinkUp_REST_API.Services.Interfaces.Pending;
using Microsoft.AspNetCore.Mvc;

namespace LinkUp_REST_API.Controllers.Pending
{
    [Route("api/[controller]")]
    [ApiController]
    public class AudienceSpecificationsController : ControllerBase
    {
        private readonly IAudienceSpecificationService _audienceSpecificationService;
        private readonly IAuthentication _authentication;

        public AudienceSpecificationsController(IAudienceSpecificationService audienceSpecificationService, IAuthentication authentication)
        {
            _audienceSpecificationService = audienceSpecificationService;
            _authentication = authentication;
        }


        //CreateAudienceSpecification



        //GetAudienceSpecificationById



        //UpdateAudienceSpecification



        //DeleteAudienceSpecificationById


    }
}
