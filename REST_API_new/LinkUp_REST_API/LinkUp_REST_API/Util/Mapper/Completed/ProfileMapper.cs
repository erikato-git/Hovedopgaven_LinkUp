using LinkUp_REST_API.DTOs.Requests.Completed;
using LinkUp_REST_API.Models;

namespace LinkUp_REST_API.Util.Mapper.Completed
{
    public class ProfileMapper
    {

        public static Profile MapToProfile(ProfileCreateInput input)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input), "ProfileCreateInput cannot be null");
            }

            var education = new Education
            {
                EducationId = Guid.NewGuid(),
                NameOfEducation = input.NameOfEducation,
                Institution = input.Institution,
                GraduationYear = input.GraduationYear,
            };

            var keyword = new Keyword
            {
                KeywordId = Guid.NewGuid(),
                Availability = input.Availability,
                YearsOfExperience = input.YearsOfExperience,
                EducationId = education.EducationId,
                Education = education,
            };

            // Link the keyword ID to education
            education.KeywordId = keyword.KeywordId;

            // Map Profile properties
            var profile = new Profile
            {
                ProfileId = Guid.NewGuid(), // Consider moving ID generation elsewhere
                Profession = input.Profession,
                Title = input.Title,
                AlternativeTitle = input.AlternativeTitle,
                ProfileDescription = input.ProfileDescription,
                AccountId = input.AccountId,
                KeywordId = keyword.KeywordId,
                Keyword = keyword,
                // TODO: portfolio
                // TODO: audienceSpecification
            };

            return profile;
        }



    }
}
