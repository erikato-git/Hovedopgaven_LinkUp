using LinkUp_REST_API.Models.Pending;
using LinkUp_REST_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinkUp_REST_API.DTOs.Requests.Completed;

namespace LinkUp_REST_API_TESTS.TestHelpers.Completed
{
    public class ProfileTestHelper
    {
        public static ProfileCreateInput GenerateValidProfileCreateInput()
        {
            return new ProfileCreateInput
            {
                Profession = "Software Developer",
                Title = "Full-Stack Engineer",
                AlternativeTitle = "Web Developer",
                ProfilePicture = null, // Replace with a mock `IFormFile` if needed for tests
                ProfileDescription = "Experienced in building modern web applications.",
                AccountId = AuthenticationTestHelper.GetValidAccountId1(),      // Supposed to be logged in user
                KeywordId = Guid.NewGuid(),
                PortfolioId = Guid.NewGuid(),
                AudienceSpecificationId = Guid.NewGuid(),
            };
        }

        public static ProfileUpdateInput GenerateValidProfileUpdateInput()
        {
            return new ProfileUpdateInput
            {
                ProfileId = Guid.NewGuid(),
                Profession = "Software Engineer",
                Title = "Senior Developer",
                AlternativeTitle = "Full Stack Developer",
                ProfilePicture = null,
                ProfileDescription = "Experienced developer specializing in web and cloud technologies.",
                AccountId = AuthenticationTestHelper.GetValidAccountId1(),
                KeywordId = Guid.NewGuid(),
                Keyword = new Keyword
                {
                    KeywordId = Guid.NewGuid(),
                },
                PortfolioId = Guid.NewGuid(),
                Portfolio = null,
                AudienceSpecificationId = Guid.NewGuid(),
                AudienceSpecification = null,
                Pitches = null
            };
        }

        public static ProfileSearchQueryInput GenerateValidSearchQueryDTO()
        {
            return new ProfileSearchQueryInput
            {
                Profession = "Software Developer",  // Default or sample profession
                Title = "Senior Engineer",          // Default or sample title
                AlternativeTitle = "Backend Developer", // Default or sample alternative title
                Age = new Random().Next(18, 65),    // Random age between 18 and 65
                YearsOfExperience = new Random().Next(0, 40), // Random years of experience between 0 and 40
                GraduationYear = DateTime.Now.Year - new Random().Next(1, 40), // Random graduation year
                Availability = "Full-Time",         // Default availability
                Institution = "University of Aarhus" // Default or sample institution
            };
        }


        public static Guid GetValidProfileId1()
        {
            return Guid.Parse("617122cf-c317-42c8-9c59-24830c640e6a");
        }

        public static Guid GetValidProfileId2()
        {
            return Guid.Parse("617122cf-c317-42c8-9c59-24830c640e6b");
        }

    }
}
