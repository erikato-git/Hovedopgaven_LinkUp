using LinkUp_REST_API.DTOs.Requests;
using LinkUp_REST_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkUp_REST_API_TESTS.TestHelpers
{
    public class AccountTestHelper
    {
        // Method to generate an invalid LoginInput object (missing or invalid properties)
        public static LoginInput GetInvalidLoginInput()
        {
            return new LoginInput
            {
                // Invalid email format or missing email
                Email = "invalid-email",   // Invalid email format
                Password = "short"         // Password too short (less than 6 characters)
            };
        }

        // Method to generate a valid LoginInput object
        public static LoginInput GetValidLoginInput()
        {
            return new LoginInput
            {
                Email = GetValidEmail(),    // Valid email format
                Password = GetValidPassword()  // Valid password (length >= 6 characters)
            };
        }

        public static string GetValidEmail()
        {
            return "user@example.com";
        }

        public static string GetValidPassword()
        {
            return "ValidPassword123";
        }


        public static Account GenerateValidAccount()
        {
            var personInformation = new PersonInformation
            {
                PersonInformationId = AuthenticationTestHelper.GetValidAccountId(),
                FirstName = "John",
                Surname = "Doe",
                Phone = "1234567890",
                BirthDate = new DateOnly(1990, 1, 1),
                Gender = "Male"
            };

            var profile = new Profile
            {
                ProfileId = Guid.NewGuid(),
                Profession = "Software Engineer",
                Title = "Senior Developer",
                AlternativeTitle = "Tech Lead",
                ProfilePicture = "https://example.com/john_doe.jpg",
                ProfileDescription = "Experienced software engineer specializing in backend development.",
            };

            var account = new Account
            {
                AccountId = Guid.NewGuid(),
                Email = "johndoe@example.com",
                Password = "SecurePassword123!", // Use secure password practices in real applications
                SavedProfileIds = new List<Guid> { profile.ProfileId },
                PersonInformationId = personInformation.PersonInformationId,
                PersonInformation = personInformation,
                Profiles = new List<Profile> { profile }
            };

            // Link the profile back to the account
            profile.AccountId = account.AccountId;
            profile.Account = account;

            return account;
        }

        public static AccountCreateInput GenerateValidAccountCreateInput()
        {
            return new AccountCreateInput
            {
                // A valid email address
                Email = "testuser@example.com",

                // A valid password (at least 6 characters long)
                Password = "Password123!",

                // A valid PersonInformationCreateDTO object
                PersonInformation = new PersonInformationCreateDTO
                {
                    // Valid first name and surname with only letters and spaces
                    FirstName = "John",
                    Surname = "Doe",

                    // A valid phone number format
                    Phone = "+1234567890",

                    // Valid birth date
                    BirthDate = new DateOnly(1990, 1, 1),

                    // A valid gender value (Male, Female, or Other)
                    Gender = "Male"
                }
            };
        }

        public static AccountUpdateInput GenerateValidAccountUpdateInput()
        {
            return new AccountUpdateInput
            {
                AccountId = AuthenticationTestHelper.GetValidAccountId(),
                Email = "valid.email@example.com",
                Password = "StrongPassword123!",
                PersonInformationId = Guid.NewGuid(),
                FirstName = "John",
                Surname = "Doe",
                Phone = "+1234567890",
                BirthDate = new DateOnly(2000, 1, 1),
                Gender = "Male"
            };
        }

    }
}
