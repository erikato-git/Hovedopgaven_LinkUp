using LinkUp_REST_API.DTOs.Requests.Completed;
using LinkUp_REST_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkUp_REST_API_TESTS.TestHelpers.Completed
{
    public class KeywordTestHelper
    {
        public static KeywordUpdateInput GenerateValidKeywordUpdateInput()
        {
            return new KeywordUpdateInput
            {
                KeywordId = GetValidKeywordId1(), // Generate a new GUID for the KeywordId
                Availability = "Part-Time", // Example availability
                YearsOfExperience = new Random().Next(1, 30), // Random years of experience between 1 and 30
                NameOfEducation = "Master of Business Administration", // Example education name
                Institution = "Business School of Copenhagen", // Example institution
                GraduationYear = DateTime.Now.Year - new Random().Next(1, 15) // Random graduation year in the last 15 years
            };
        }

        public static KeywordCreateInput GenerateValidKeyword()
        {
            return new KeywordCreateInput
            {
                Availability = "Full-Time", // Example availability
                YearsOfExperience = new Random().Next(1, 20), // Random years of experience between 1 and 20
                ProfileId = ProfileTestHelper.GetValidProfileId1(), // Generate a new GUID for ProfileId
                NameOfEducation = "Bachelor of Science in Computer Science", // Example education name
                Institution = "University of Aarhus", // Example institution
                GraduationYear = DateTime.Now.Year - new Random().Next(1, 10) // Random graduation year in the last 10 years
            };
        }

        public static KeywordCreateInput GenerateValidKeywordToProfileWihoutKeyword()
        {
            return new KeywordCreateInput
            {
                Availability = "Full-Time", // Example availability
                YearsOfExperience = new Random().Next(1, 20), // Random years of experience between 1 and 20
                ProfileId = ProfileTestHelper.GetValidProfileId3(), // Generate a new GUID for ProfileId
                NameOfEducation = "Bachelor of Science in Computer Science", // Example education name
                Institution = "University of Aarhus", // Example institution
                GraduationYear = DateTime.Now.Year - new Random().Next(1, 10) // Random graduation year in the last 10 years
            };
        }


        public static Guid GetValidKeywordId1()
        {
            return Guid.Parse("2c9dd441-e776-4087-9613-22372d1cc991");
        }


        public static Guid GetValidKeywordId2()
        {
            return Guid.Parse("2c9dd441-e776-4087-9613-22372d1cc992");
        }


    }
}
