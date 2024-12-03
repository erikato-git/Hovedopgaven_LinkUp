using LinkUp_REST_API.DTOs.Requests;
using LinkUp_REST_API.Models;
using LinkUp_REST_API_TESTS.TestHelpers.Completed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkUp_REST_API_TESTS.TestHelpers
{
    public class KeywordTestHelper
    {
        public static KeywordCreateUpdateInput GenerateValidKeywordCreateUpdateInput()
        {
            return new KeywordCreateUpdateInput
            {
                Availability = "Full-Time", // Example availability
                YearsOfExperience = new Random().Next(1, 20), // Random years of experience between 1 and 20
                ProfileId = ProfileTestHelper.GetValidProfileId1(), // Generate a new GUID for ProfileId
                EducationId = Guid.NewGuid(), // Generate a new GUID for EducationId
                Education = new Education
                {
                    EducationId = Guid.NewGuid(), // Generate a new GUID for Education
                    NameOfEducation = "Bachelor of Science in Computer Science", // Example education name
                    Institution = "University of Aarhus", // Example institution
                    GraduationYear = DateTime.Now.Year - new Random().Next(1, 10) // Random graduation year in the last 10 years
                }
            };
        }

        public static Guid GetValidKeywordId1()
        {
            return Guid.Parse("2c9dd441-e776-4087-9613-22372d1cc991");
        }





    }
}
