using LinkUp_REST_API.DTOs.Requests;
using LinkUp_REST_API.Models;

namespace LinkUp_REST_API.Util.Mapper
{
    public class KeywordMapper
    {
        public static Keyword MapToKeyword(KeywordCreateInput input)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input), "The input DTO cannot be null.");
            }

            return new Keyword
            {
                KeywordId = Guid.NewGuid(),
                Availability = input.Availability,
                YearsOfExperience = input.YearsOfExperience,
                ProfileId = input.ProfileId,
                Education = input.Education
            };
        }


    }
}


