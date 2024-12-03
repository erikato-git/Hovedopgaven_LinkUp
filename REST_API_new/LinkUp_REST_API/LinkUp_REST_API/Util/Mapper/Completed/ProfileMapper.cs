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

            return new Profile
            {
                ProfileId = Guid.NewGuid(),             // Consider to put generating of Ids another place, it's a huge responsibility
                Profession = input.Profession,
                Title = input.Title,
                AlternativeTitle = input.AlternativeTitle,
                //ProfilePicture = input.ProfilePicture,            // will be added later
                ProfileDescription = input.ProfileDescription,
                KeywordId = input.KeywordId,
                Keyword = input.Keyword,
                AccountId = input.AccountId,
                PortfolioId = input.PortfolioId,
                Portfolio = input.Portfolio,
                AudienceSpecificationId = input.AudienceSpecificationId,
                AudienceSpecification = input.AudienceSpecification
            };
        }



    }
}
