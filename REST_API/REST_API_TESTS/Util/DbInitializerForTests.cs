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
            dbContext.Accounts.AddRange(GetAccountsForTest());
            dbContext.Profiles.AddRange(GetProfilesForTest());      // saves into in-memory
            dbContext.SaveChanges();                                // saves into test-container database
        }

        public static void ReinitDbForTests(MssqlDbContext dbContext)
        {
            dbContext.Accounts.RemoveRange(dbContext.Accounts);
            dbContext.Profiles.RemoveRange(dbContext.Profiles);     // removes all items for the table
            dbContext.SaveChanges();    
            InitDbForTests(dbContext);                              // re-init new items
        }



        private static List<Profile> GetProfilesForTest()
        {
            return new List<Profile>
            {
                new Profile
                {
                    ProfileId = Guid.NewGuid(),
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
                    ProfileId = Guid.NewGuid(),
                    Profession = "Graphic Designer",
                    Title = "Creative Specialist",
                    ProfilePicture = "profile2.jpg",
                    ProfileDescription = "Specialized in UI/UX design",
                    AccountId = Guid.Parse("22222222-2222-2222-2222-222222222222"), // Links to another Account
                    KeywordId = Guid.NewGuid(),
                    PortfolioId = Guid.NewGuid(),
                    AudienceSpecificationId = null, // No audience specification
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
                    Profiles = new List<Profile>(), // Populated after Profiles are added
                    SavedProfileIds = null
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
                    Profiles = null, // Populated after Profiles are added
                    SavedProfileIds = new List<Profile>() // Referencing saved profiles
                }
            };
        }




    }
}
