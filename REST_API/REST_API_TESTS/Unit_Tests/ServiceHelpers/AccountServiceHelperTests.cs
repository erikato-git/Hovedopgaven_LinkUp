using AutoFixture;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using Moq;
using REST_API.Repositories.Interfaces;
using REST_API.Services.Domains;
using REST_API.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REST_API_TESTS.Unit_Tests.ServiceHelpers
{
    public class AccountServiceHelperTests
    {
        private readonly Mock<IAccountRepository> _accountRepository;
        private readonly Fixture _fixture;
        private readonly AccountServiceHelper _sut;

        public AccountServiceHelperTests()
        {
            _fixture = new Fixture();
            _accountRepository = new Mock<IAccountRepository>();

            _sut = new AccountServiceHelper(_accountRepository.Object);

        }

    }
}
