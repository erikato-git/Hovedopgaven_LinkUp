using LinkUp_REST_API.Core.Interfaces;
using LinkUp_REST_API.Services.Interfaces;
using LinkUp_REST_API.Services.Interfaces.Pending;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LinkUp_REST_API.Controllers.Pending
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonInformationController : ControllerBase
    {
        private IPersonInformationService _personInformationService;
        private IAuthentication _authentication;

        public PersonInformationController(IPersonInformationService personInformationService, IAuthentication authentication)
        {
            _personInformationService = personInformationService;
            _authentication = authentication;
        }


        //CreatePersonInformation


        //GetPersonInformationById


        //UpdatePersonInformation


        //DeletePersonInformationById




    }
}
