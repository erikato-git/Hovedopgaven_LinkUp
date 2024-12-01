using Microsoft.EntityFrameworkCore;
using REST_API.Data;
using REST_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REST_API_TESTS.Util
{
    /*
     * Reference: Cummings, Neil: "Build a Microservice app with .NET and NextJS from scratch", lecture: 193, Udemy
     */

    public class DbInitializerForTests
    {
        public static void InitDbForTests(MssqlDbContext dbContext)
        {
            dbContext.Accounts.AddRange(GetAccountsForTest());      // saves into in-memory
            dbContext.Profiles.AddRange(GetProfilesForTest());
            dbContext.Pitches.AddRange(GetPitchesForTest());
            dbContext.Keywords.AddRange(GetKeywordsForTest());
            dbContext.Educations.AddRange(GetEducationsForTest());
            dbContext.SaveChanges();                                // saves into test-container database
        }

        public static void ReinitDbForTests(MssqlDbContext dbContext)
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
                    ProfileId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                    Profession = "Software Engineer",
                    Title = "Full Stack Developer",
                    AlternativeTitle = "Tech Enthusiast",
                    ProfilePicture = "profile1.jpg",
                    ProfileDescription = "Experienced in building web applications",
                    AccountId = Guid.Parse("11111111-1111-1111-1111-111111111111"), // Links to an Account
                    KeywordId = Guid.NewGuid(),
                    PortfolioId = Guid.NewGuid(),
                    AudienceSpecificationId = Guid.NewGuid(),
                    Pitches = null
                },
                new Profile
                {
                    ProfileId = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                    Profession = "Graphic Designer",
                    Title = "Creative Specialist",
                    ProfilePicture = "profile2.jpg",
                    ProfileDescription = "Specialized in UI/UX design",
                    AccountId = Guid.Parse("22222222-2222-2222-2222-222222222222"), // Links to another Account
                    KeywordId = Guid.NewGuid(),
                    PortfolioId = Guid.NewGuid(),
                    AudienceSpecificationId = null, // No audience specification
                    Pitches = null
                },
                new Profile
                {
                    ProfileId = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                    Profession = "Marketing Manager",
                    Title = "Digital Marketer",
                    ProfilePicture = "profile3.jpg",
                    ProfileDescription = "Expert in SEO and digital campaigns",
                    AccountId = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                    KeywordId = Guid.NewGuid(),
                    PortfolioId = Guid.NewGuid(),
                    AudienceSpecificationId = Guid.NewGuid(),
                    Pitches = null
                },
                new Profile
                {
                    ProfileId = Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddddddd"),
                    Profession = "Data Scientist",
                    Title = "AI Researcher",
                    AlternativeTitle = "Machine Learning Expert",
                    ProfilePicture = "profile4.jpg",
                    ProfileDescription = "Experienced in building predictive models",
                    AccountId = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                    KeywordId = Guid.NewGuid(),
                    PortfolioId = Guid.NewGuid(),
                    AudienceSpecificationId = Guid.NewGuid(),
                    Pitches = null
                }
            };
        }


        private static List<Account> GetAccountsForTest()
        {
            return new List<Account>
            {
                new Account
                {
                    AccountId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    Email = "user1@example.com",
                    Password = "password123", // Normally hashed in production
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
                    SavedProfileIds = new List<Guid>()
                },
                new Account
                {
                    AccountId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
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
                    SavedProfileIds = new List<Guid>()
                },
                new Account
                {
                    AccountId = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                    Email = "user3@example.com",
                    Password = "password789",
                    PersonInformationId = Guid.NewGuid(),
                    PersonInformation = new PersonInformation
                    {
                        PersonInformationId = Guid.NewGuid(),
                        FirstName = "Charlie",
                        Surname = "Brown",
                        Phone = "5678901234",
                        BirthDate = DateOnly.FromDateTime(DateTime.Now.AddYears(-28)),
                        Gender = "Non-Binary"
                    },
                    Profiles = new List<Profile>(),
                    SavedProfileIds = new List<Guid>()
                },
                new Account
                {
                    AccountId = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                    Email = "user4@example.com",
                    Password = "password000",
                    PersonInformationId = Guid.NewGuid(),
                    PersonInformation = new PersonInformation
                    {
                        PersonInformationId = Guid.NewGuid(),
                        FirstName = "Diana",
                        Surname = "Prince",
                        Phone = "5432167890",
                        BirthDate = DateOnly.FromDateTime(DateTime.Now.AddYears(-35)),
                        Gender = "Female"
                    },
                    Profiles = new List<Profile>(),
                    SavedProfileIds = new List<Guid>()
                }
            };
        }

        private static List<Pitch> GetPitchesForTest()
        {
            return new List<Pitch>
            {
                new Pitch
                {
                    PitchId = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                    SendingDate = DateTime.UtcNow,
                    TextMessage = "This is a test pitch for profile 1",
                    RecipientProfileId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), // Links to Profile 1
                    RecipientAccountId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    ProfileId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                    Profile = null
                },
                new Pitch
                {
                    PitchId = Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddddddd"),
                    SendingDate = DateTime.UtcNow.AddMinutes(-30),
                    TextMessage = "This is a test pitch for profile 2",
                    RecipientProfileId = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), // Links to Profile 2
                    RecipientAccountId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    ProfileId = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                    Profile = null
                },
                new Pitch
                {
                    PitchId = Guid.Parse("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"),
                    SendingDate = DateTime.UtcNow.AddDays(-1),
                    TextMessage = "This is a test pitch for profile 3",
                    RecipientProfileId = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                    RecipientAccountId = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                    ProfileId = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                    Profile = null
                },
                new Pitch
                {
                    PitchId = Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff"),
                    SendingDate = DateTime.UtcNow.AddHours(-12),
                    TextMessage = "This is a test pitch for profile 4",
                    RecipientProfileId = Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddddddd"),
                    RecipientAccountId = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                    ProfileId = Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddddddd"),
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
                    KeywordId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    Availability = "Full-time",
                    YearsOfExperience = 5,
                    ProfileId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                    EducationId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), // Links to Education 1
                    Profile = null,
                    Education = null
                },
                new Keyword
                {
                    KeywordId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    Availability = "Freelance",
                    YearsOfExperience = 3,
                    ProfileId = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                    EducationId = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), // Links to Education 2
                    Profile = null,
                    Education = null
                },
                new Keyword
                {
                    KeywordId = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                    Availability = "Part-time",
                    YearsOfExperience = 7,
                    ProfileId = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                    EducationId = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc"), // Links to Education 3
                    Profile = null,
                    Education = null
                },
                new Keyword
                {
                    KeywordId = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                    Availability = "Available on-demand",
                    YearsOfExperience = 10,
                    ProfileId = Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddddddd"),
                    EducationId = Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddddddd"), // Links to Education 4
                    Profile = null,
                    Education = null
                }
            };
        }

        private static List<Education> GetEducationsForTest()
        {
            return new List<Education>
            {
                new Education
                {
                    EducationId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), // Matches Keyword 1
                    NameOfEducation = "Bachelor of Computer Science",
                    Institution = "University of Technology",
                    GraduationYear = "2015",
                    KeywordId = Guid.Parse("11111111-1111-1111-1111-111111111111"), // Links back to Keyword 1
                    Keyword = null
                },
                new Education
                {
                    EducationId = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), // Matches Keyword 2
                    NameOfEducation = "Master of Design",
                    Institution = "School of Creative Arts",
                    GraduationYear = "2018",
                    KeywordId = Guid.Parse("22222222-2222-2222-2222-222222222222"), // Links back to Keyword 2
                    Keyword = null
                },
                new Education
                {
                    EducationId = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc"), // Matches Keyword 3
                    NameOfEducation = "PhD in Machine Learning",
                    Institution = "Institute of AI",
                    GraduationYear = "2021",
                    KeywordId = Guid.Parse("33333333-3333-3333-3333-333333333333"), // Links back to Keyword 3
                    Keyword = null
                },
                new Education
                {
                    EducationId = Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddddddd"), // Matches Keyword 4
                    NameOfEducation = "Diploma in Data Science",
                    Institution = "Tech Academy",
                    GraduationYear = "2017",
                    KeywordId = Guid.Parse("44444444-4444-4444-4444-444444444444"), // Links back to Keyword 4
                    Keyword = null
                }
            };
        }

    }
}
