using REST_API.DTOs;
using REST_API.DTOs.AccountDomain;
using REST_API.DTOs.ProfileDomain;
using REST_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REST_API_TESTS.Helpers
{
    public static class ProfileTestHelper
    {
        public static CreateProfileDTO GenerateValidCreateProfileDTO()
        {
            return new CreateProfileDTO()
            {
                ProfileId = Guid.NewGuid(),
                Profession = "Software Developer",
                Title = "Senior Developer",
                AlternativeTitle = "Backend Engineer",
                ProfilePicture = "https://example.com/images/profile-picture.jpg",
                ProfileDescription = "Experienced developer specializing in .NET and cloud technologies.",
                AccountId = Guid.NewGuid(),
                KeywordId = Guid.NewGuid(),
                PortfolioId = Guid.NewGuid(),
                AudienceSpecificationId = Guid.NewGuid(),
                PitchIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() }
            };
        }

        public static UpdateProfileDTO GenerateValidUpdateProfileDTO()
        {
            return new UpdateProfileDTO()
            {
                ProfileId = Guid.NewGuid(),
                Profession = "Software Developer",
                Title = "Senior Developer",
                AlternativeTitle = "Backend Engineer",
                ProfilePicture = "https://example.com/images/profile-picture.jpg",
                ProfileDescription = "Experienced developer specializing in .NET and cloud technologies.",
                AccountId = Guid.NewGuid(),
            };
        }

        public static SearchQueryDTO GenerateValidSearchQueryDTO()
        {
            return new SearchQueryDTO
            {
                Profession = "Software Engineer",
                Title = "Senior Developer",
                AlternativeTitle = "Backend Specialist",
                Age = 30,
                YearsOfExperience = 8,
                Institution = "Tech University",
                GraduationYear = 2015,
                Availability = "Available",
            };
        }



        public static Profile GenerateValidProfile()
        {
            // Example PersonInformation object
            var personInfo = new PersonInformation
            {
                PersonInformationId = Guid.NewGuid(),
                FirstName = "John",
                Surname = "Doe",
                Phone = "123-456-7890",
                BirthDate = new DateOnly(1999, 12, 31),
                Gender = "Male"
            };

            // Example Account object
            var account = new Account
            {
                AccountId = Guid.NewGuid(),
                Email = "john.doe@example.com",
                Password = "hashed_password",
                PersonInformationId = personInfo.PersonInformationId,
                PersonInformation = personInfo
            };

            // Example Education object
            var education = new Education
            {
                EducationId = Guid.NewGuid(),
                NameOfEducation = "Bachelor of Computer Science",
                Institution = "Tech University",
                GraduationYear = "2022",
                KeywordId = Guid.NewGuid() // Will link this to Keyword later
            };

            // Example Keyword object
            var keyword = new Keyword
            {
                KeywordId = education.KeywordId,
                Availability = "Full-time",
                YearsOfExperience = 3,
                ProfileId = Guid.NewGuid(), // Will link this to Profile later
                Education = education,
                EducationId = education.EducationId
            };

            // Example Portfolio object
            var portfolio = new Portfolio
            {
                PortfolioId = Guid.NewGuid(),
                Projects = new List<string> { "Project A", "Project B" },
                ProfileId = Guid.NewGuid() // Will link this to Profile later
            };

            // Example Profile object
            var profile = new Profile
            {
                ProfileId = Guid.NewGuid(),
                Profession = "Software Developer",
                Title = "Senior Developer",
                AlternativeTitle = "Full-Stack Engineer",
                ProfilePicture = "https://example.com/profile-pic.jpg",
                ProfileDescription = "Experienced in building scalable web applications.",
                AccountId = account.AccountId,
                Account = account,
                KeywordId = keyword.KeywordId,
                Keyword = keyword,
                PortfolioId = portfolio.PortfolioId,
                Portfolio = portfolio,
                AudienceSpecificationId = Guid.NewGuid(),
                AudienceSpecification = null, // Assuming it's optional for now
                Pitches = new List<Pitch> // Example pitches
                {
                    new Pitch
                    {
                        PitchId = Guid.NewGuid(),
                        TextMessage = "Pitch for Project X",
                        ProfileId = Guid.NewGuid() // Will link this to Profile later
                    }
                }
            };

            // Linking objects
            keyword.ProfileId = profile.ProfileId;
            portfolio.ProfileId = profile.ProfileId;

            return profile;
        }

    }

}
