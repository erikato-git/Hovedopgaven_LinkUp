using AutoFixture;
using Moq;
using REST_API.Repositories;
using REST_API.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REST_API_TESTS.Unit_Tests.Repositories
{
    public class AccountRepositoryTests
    {
        private readonly Mock<IAccountRepository> _accountRepository;
        private readonly Fixture _fixture;
        private readonly AccountRepository _sut;

        public AccountRepositoryTests()
        {
            _fixture = new Fixture();
            _accountRepository = new Mock<IAccountRepository>();

            // TODO: vent med at teste repositories til at jeg har studeret brug af test-containers noget mere

            //_sut = new AccountRepository();
        }
    }
}
