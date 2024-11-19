using REST_API.DTOs;
using REST_API.DTOs.AccountDomain;
using REST_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/*
* AutoFixture didn't work to create fakes for complex objects
*/

namespace REST_API_TESTS.Helpers
{
    public static class AccountTestHelper
    {
        public static Account GenerateValidFakeAccount()
        {
            var personInfo = new PersonInformation
            {
                PersonInformationId = Guid.NewGuid(),
                FirstName = "John",
                Surname = "Doe",
                Phone = "123-456-7890",
                BirthDate = new DateOnly(1990, 5, 20),
                Gender = "Male"
            };

            return new Account
            {
                AccountId = Guid.NewGuid(),
                Email = "johndoe@example.com",
                Password = "SecurePassword123", // In real applications, passwords should be hashed
                PersonInformationId = personInfo.PersonInformationId,
                PersonInformation = personInfo,
                Profiles = new List<Profile> // Assuming the Profile class exists
                {
                    new Profile { }
                }
            };
        }

        public static Account GenerateValidFakeAccountWithoutAnyProfiles()
        {
            var personInfo = new PersonInformation
            {
                PersonInformationId = Guid.NewGuid(),
                FirstName = "John",
                Surname = "Doe",
                Phone = "123-456-7890",
                BirthDate = new DateOnly(1990, 5, 20),
                Gender = "Male"
            };

            return new Account
            {
                AccountId = Guid.NewGuid(),
                Email = "johndoe@example.com",
                Password = "SecurePassword123", // In real applications, passwords should be hashed
                PersonInformationId = personInfo.PersonInformationId,
                PersonInformation = personInfo,
                Profiles = new List<Profile> // Assuming the Profile class exists
                {

                }
            };
        }



        public static LoginDTO GenerateFakeInvalidLoginDTO()
        {
            return new LoginDTO
            {
                Email = string.Empty,
                Password = string.Empty,
            };
        }

        public static CreateAccountDTO GenerateFakeInvalidCreateAccountDTO()
        {
            return new CreateAccountDTO
            {
                FirstName = string.Empty,
                Surname = string.Empty,
                Email = string.Empty,
                Phone = string.Empty,
                BirthDate = DateTime.MinValue,
                Gender = string.Empty,
                Password = string.Empty
            };
        }

        public static UpdateAccountDTO GenerateFakeInvalidUpdateAccountDTO()
        {
            return new UpdateAccountDTO
            {
                AccountId = Guid.Empty,
                FirstName = null,
                Surname = null,
                Email = null,
                Phone = null,
                BirthDate = null,
                Gender = null,
                Password = null
            };
        }

        public static UpdateAccountDTO GenerateFakeValidUpdateAccountDTO()
        {
            return new UpdateAccountDTO
            {
                AccountId = Guid.NewGuid(), // Required field

                // Optional fields (include if you want to update them)
                FirstName = "Alice",
                Surname = "Smith",
                Email = "alice.smith@example.com",
                Phone = "+1234567890",
                BirthDate = new DateTime(1995, 5, 20),
                Gender = "Female",
                Password = "Secure@1234" // Meets all password criteria
            };
        }


    }

}
