using REST_API.DTOs.ProfileDomain;
using REST_API.Models;

namespace REST_API.Util.Mapper
{
    public class ProfileMapper
    {
        // CreateProfileDTO -> Profile
        public static Profile MapCreateProfileDTOToProfile(CreateProfileDTO createProfileDto)
        {
            if (createProfileDto == null)
            {
                throw new ArgumentNullException(nameof(createProfileDto), "CreateProfileDTO cannot be null");
            }

            return new Profile
            {
                ProfileId = Guid.NewGuid(),
                Profession = createProfileDto.Profession,
                Title = createProfileDto.Title,
                AlternativeTitle = createProfileDto.AlternativeTitle,
                ProfilePicture = createProfileDto.ProfilePicture,
                ProfileDescription = createProfileDto.ProfileDescription,
                AccountId = createProfileDto.AccountId,
                KeywordId = createProfileDto.KeywordId,
                PortfolioId = createProfileDto.PortfolioId,
                AudienceSpecificationId = createProfileDto.AudienceSpecificationId,
            };
        }


        // UpdateProfileDTO -> Profile
        public static Profile MapUpdateProfileDTOToProfile(UpdateProfileDTO updateProfileDto, Profile existingProfile)
        {
            if (updateProfileDto == null)
            {
                throw new ArgumentNullException(nameof(updateProfileDto), "UpdateProfileDTO cannot be null");
            }

            if (existingProfile == null)
            {
                throw new ArgumentNullException(nameof(existingProfile), "Existing Profile cannot be null");
            }

            // Update fields only if they have non-default values in the DTO
            if (!string.IsNullOrEmpty(updateProfileDto.Profession))
            {
                existingProfile.Profession = updateProfileDto.Profession;
            }

            if (!string.IsNullOrEmpty(updateProfileDto.Title))
            {
                existingProfile.Title = updateProfileDto.Title;
            }

            if (!string.IsNullOrEmpty(updateProfileDto.AlternativeTitle))
            {
                existingProfile.AlternativeTitle = updateProfileDto.AlternativeTitle;
            }

            if (!string.IsNullOrEmpty(updateProfileDto.ProfilePicture))
            {
                existingProfile.ProfilePicture = updateProfileDto.ProfilePicture;
            }

            if (!string.IsNullOrEmpty(updateProfileDto.ProfileDescription))
            {
                existingProfile.ProfileDescription = updateProfileDto.ProfileDescription;
            }

            return existingProfile;
        }
    }
}
