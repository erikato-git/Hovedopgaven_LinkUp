using LinkUp_REST_API.Core.Interfaces;
using LinkUp_REST_API.Services.Interfaces.Pending;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LinkUp_REST_API.Controllers.Pending
{
    [Route("api/[controller]")]
    [ApiController]
    public class EducationsController : ControllerBase
    {
        private IEducationService _educationService;
        private IAuthentication _authentication;

        public EducationsController(IEducationService educationService, IAuthentication authentication)
        {
            _educationService = educationService;
            _authentication = authentication;
        }


        //CreateEducation


        //GetEducationById


        //UpdateEducation


        //DeleteEducationById

    }
}
