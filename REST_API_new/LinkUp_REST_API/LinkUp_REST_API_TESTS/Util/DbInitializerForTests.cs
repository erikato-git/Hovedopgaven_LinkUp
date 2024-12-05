using CloudinaryDotNet;
using LinkUp_REST_API.Core;
using LinkUp_REST_API.Data.DbContextConnections;
using LinkUp_REST_API.Models;
using LinkUp_REST_API_TESTS.TestHelpers.Completed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Account = LinkUp_REST_API.Models.Account;

namespace LinkUp_REST_API_TESTS.Util
{
    public class DbInitializerForTests
    {
        public static void InitDbForTests(DataContext dbContext)
        {
            dbContext.Accounts.AddRange(GetAccountsForTest());      // saves into in-memory
            dbContext.Profiles.AddRange(GetProfilesForTest());
            dbContext.Pitches.AddRange(GetPitchesForTest());
            dbContext.Keywords.AddRange(GetKeywordsForTest());
            dbContext.Educations.AddRange(GetEducationsForTest());
            dbContext.SaveChanges();                                // saves into test-container database
        }

        public static void ReinitDbForTests(DataContext dbContext)
        {
            dbContext.Accounts.RemoveRange(dbContext.Accounts);
            dbContext.Profiles.RemoveRange(dbContext.Profiles);
            dbContext.Pitches.RemoveRange(dbContext.Pitches);
            dbContext.Keywords.RemoveRange(dbContext.Keywords);
            dbContext.Educations.RemoveRange(dbContext.Educations);
            dbContext.SaveChanges();
            InitDbForTests(dbContext);                              // re-init new items
        }


        private static List<Profile> GetProfilesForTest()
        {
            return new List<Profile>
            {
                new Profile
                {
                    ProfileId = ProfileTestHelper.GetValidProfileId1(),
                    Profession = "Software Engineer",
                    Title = "Full Stack Developer",
                    AlternativeTitle = "Tech Enthusiast",
                    ProfilePicture = null,
                    ProfileDescription = "Experienced in building web applications",
                    AccountId = AuthenticationTestHelper.GetValidAccountId1(), // Links to an Account
                    KeywordId = KeywordTestHelper.GetValidKeywordId1(),
                    PortfolioId = Guid.NewGuid(),
                    AudienceSpecificationId = Guid.NewGuid(),
                    Pitches = null
                },
                new Profile
                {
                    ProfileId = ProfileTestHelper.GetValidProfileId2(),
                    Profession = "Graphic Designer",
                    Title = "Creative Specialist",
                    ProfilePicture = null,
                    ProfileDescription = "Specialized in UI/UX design",
                    AccountId = Guid.Parse("22222222-2222-2222-2222-222222222222"), // Links to another Account
                    KeywordId = KeywordTestHelper.GetValidKeywordId2(),
                    PortfolioId = Guid.NewGuid(),
                    AudienceSpecificationId = null, // No audience specification
                    Pitches = null
                },
                new Profile
                {
                    ProfileId = ProfileTestHelper.GetValidProfileId3(),
                    Profession = "Marketing Manager",
                    Title = "Digital Marketer",
                    ProfilePicture = null,
                    ProfileDescription = "Expert in SEO and digital campaigns",
                    AccountId = AuthenticationTestHelper.GetValidAccountId1(),
                    KeywordId = null,
                    PortfolioId = null,
                    AudienceSpecificationId = Guid.NewGuid(),
                    Pitches = null
                },
                //new Profile
                //{
                //    ProfileId = Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddddddd"),
                //    Profession = "Data Scientist",
                //    Title = "AI Researcher",
                //    AlternativeTitle = "Machine Learning Expert",
                //    ProfilePicture = "profile4.jpg",
                //    ProfileDescription = "Experienced in building predictive models",
                //    AccountId = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                //    KeywordId = Guid.NewGuid(),
                //    PortfolioId = Guid.NewGuid(),
                //    AudienceSpecificationId = Guid.NewGuid(),
                //    Pitches = null
                //}

            };
        }


        private static List<Account> GetAccountsForTest()
        {
            return new List<Account>
            {
                new Account
                {
                    AccountId = AuthenticationTestHelper.GetValidAccountId1(),
                    Email = AccountTestHelper.GetValidEmail(),
                    Password = Authentication.HashingPasswordWithSaltUsingSHA256(AccountTestHelper.GetValidPassword(), AuthenticationTestHelper.GetValidAccountId1()), // Normally hashed in production
                    PersonInformationId = Guid.NewGuid(),
                    PersonInformation = new PersonInformation
                    {
                        PersonInformationId = Guid.NewGuid(),
                        FirstName = "Alice",
                        Surname = "Johnson",
                        Phone = "1234567890",
                        BirthDate = DateOnly.FromDateTime(DateTime.Now.AddYears(-30)),
                        Gender = "Female"
                    },
                    Profiles = new List<Profile>(),
                    FavoriteProfiles = new List<Guid>() { ProfileTestHelper.GetValidProfileId1() }
                },
                new Account
                {
                    AccountId = AuthenticationTestHelper.GetValidAccountId2(),
                    Email = "user2@example.com",
                    Password = "password456",
                    PersonInformationId = Guid.NewGuid(),
                    PersonInformation = new PersonInformation
                    {
                        PersonInformationId = Guid.NewGuid(),
                        FirstName = "Bob",
                        Surname = "Smith",
                        Phone = "0987654321",
                        BirthDate = DateOnly.FromDateTime(DateTime.Now.AddYears(-25)),
                        Gender = "Male"
                    },
                    Profiles = new List<Profile>(),
                    FavoriteProfiles = new List<Guid>()
                },
                //new Account
                //{
                //    AccountId = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                //    Email = "user3@example.com",
                //    Password = "password789",
                //    PersonInformationId = Guid.NewGuid(),
                //    PersonInformation = new PersonInformation
                //    {
                //        PersonInformationId = Guid.NewGuid(),
                //        FirstName = "Charlie",
                //        Surname = "Brown",
                //        Phone = "5678901234",
                //        BirthDate = DateOnly.FromDateTime(DateTime.Now.AddYears(-28)),
                //        Gender = "Non-Binary"
                //    },
                //    Profiles = new List<Profile>(),
                //    SavedProfileIds = new List<Guid>()
                //},
                //new Account
                //{
                //    AccountId = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                //    Email = "user4@example.com",
                //    Password = "password000",
                //    PersonInformationId = Guid.NewGuid(),
                //    PersonInformation = new PersonInformation
                //    {
                //        PersonInformationId = Guid.NewGuid(),
                //        FirstName = "Diana",
                //        Surname = "Prince",
                //        Phone = "5432167890",
                //        BirthDate = DateOnly.FromDateTime(DateTime.Now.AddYears(-35)),
                //        Gender = "Female"
                //    },
                //    Profiles = new List<Profile>(),
                //    SavedProfileIds = new List<Guid>()
                //}
            
            };
        }

        private static List<Pitch> GetPitchesForTest()
        {
            return new List<Pitch>
            {
                new Pitch
                {
                    PitchId = PitchTestHelper.GetValidPitchId1(),
                    SendingDate = DateTime.UtcNow,
                    TextMessage = "This is a test pitch for profile 1",
                    RecipientProfileId = ProfileTestHelper.GetValidProfileId1(), // Links to Profile 1
                    ProfileId = ProfileTestHelper.GetValidProfileId2(),
                    Profile = null
                },
                new Pitch
                {
                    PitchId = PitchTestHelper.GetValidPitchId2(),
                    SendingDate = DateTime.UtcNow.AddMinutes(-30),
                    TextMessage = "This is a test pitch for profile 2",
                    RecipientProfileId = ProfileTestHelper.GetValidProfileId2(), // Links to Profile 1
                    ProfileId = ProfileTestHelper.GetValidProfileId1(),
                    Profile = null
                },
                new Pitch
                {
                    PitchId = Guid.Parse("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"),
                    SendingDate = DateTime.UtcNow.AddDays(-1),
                    TextMessage = "This is a test pitch for profile 3",
                    RecipientProfileId = ProfileTestHelper.GetValidProfileId2(),
                    ProfileId = ProfileTestHelper.GetValidProfileId1(),
                    Profile = null
                },
                new Pitch
                {
                    PitchId = PitchTestHelper.GetPitchIdWithNoAssociated(),
                    SendingDate = DateTime.UtcNow.AddHours(-12),
                    TextMessage = "This is a test pitch for profile 4",
                    RecipientProfileId = Guid.NewGuid(),
                    ProfileId = ProfileTestHelper.GetValidProfileId2(),
                    Profile = null
                }

            };
        }


        private static List<Keyword> GetKeywordsForTest()
        {
            return new List<Keyword>
            {
                new Keyword
                {
                    KeywordId = KeywordTestHelper.GetValidKeywordId1(),
                    Availability = "Full-time",
                    YearsOfExperience = 5,
                    ProfileId = ProfileTestHelper.GetValidProfileId1(),
                    EducationId = null, // Links to Education 1
                    Profile = null,
                    Education = null
                },
                new Keyword
                {
                    KeywordId = KeywordTestHelper.GetValidKeywordId2(),
                    Availability = "Freelance",
                    YearsOfExperience = 3,
                    ProfileId = ProfileTestHelper.GetValidProfileId2(),
                    EducationId = null,
                    Profile = null,
                    Education = null
                }
            
            };
        }

        private static List<Education> GetEducationsForTest()
        {
            return new List<Education>
            {
                //new Education
                //{
                //    EducationId = EducationTestHelper.GetValidEducationId1(), // Matches Keyword 1
                //    NameOfEducation = "Bachelor of Computer Science",
                //    Institution = "University of Technology",
                //    GraduationYear = 2015,
                //    KeywordId = KeywordTestHelper.GetValidKeywordId1(), // Links back to Keyword 1
                //    Keyword = null
                //},
                //new Education
                //{
                //    EducationId = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), // Matches Keyword 2
                //    NameOfEducation = "Master of Design",
                //    Institution = "School of Creative Arts",
                //    GraduationYear = "2018",
                //    KeywordId = Guid.Parse("22222222-2222-2222-2222-222222222222"), // Links back to Keyword 2
                //    Keyword = null
                //},
                //new Education
                //{
                //    EducationId = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc"), // Matches Keyword 3
                //    NameOfEducation = "PhD in Machine Learning",
                //    Institution = "Institute of AI",
                //    GraduationYear = "2021",
                //    KeywordId = Guid.Parse("33333333-3333-3333-3333-333333333333"), // Links back to Keyword 3
                //    Keyword = null
                //},
                //new Education
                //{
                //    EducationId = Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddddddd"), // Matches Keyword 4
                //    NameOfEducation = "Diploma in Data Science",
                //    Institution = "Tech Academy",
                //    GraduationYear = "2017",
                //    KeywordId = Guid.Parse("44444444-4444-4444-4444-444444444444"), // Links back to Keyword 4
                //    Keyword = null
                //}
            
            };
        }

    }

}
