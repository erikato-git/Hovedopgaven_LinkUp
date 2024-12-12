using LinkUp_REST_API.Core;
using LinkUp_REST_API.DTOs.Requests.Completed;
using LinkUp_REST_API.DTOs.Responses.Completed;
using LinkUp_REST_API.Repositories.Interfaces.Completed;
using LinkUp_REST_API.Services.Interfaces.Completed;
using LinkUp_REST_API.Util;
using LinkUp_REST_API.Util.Mapper.Completed;

namespace LinkUp_REST_API.Services.Completed
{
    public class ProfileService : IProfileService
    {
        private readonly IProfileRepository _profileRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IProfileServiceHelper _helper;
        public ProfileService(IAccountRepository accountRepository, IProfileRepository profileRepository, IProfileServiceHelper helper)
        {
            _accountRepository = accountRepository;
            _profileRepository = profileRepository;
            _helper = helper;
        }


        public async Task<ResultDTO> RemoveProfileFromFavorites(Guid profileId, string userAccountId)
        {
            if (string.IsNullOrEmpty(profileId.ToString()) || string.IsNullOrEmpty(userAccountId))
            {
                return ResultDTO.Failure(400, "Invalid inputs");
            }

            // Check logged in accounts favorites contains profileId
            var loggedInUser = await _accountRepository.GetByIdAsync(Guid.Parse(userAccountId));

            if (loggedInUser == null)
            {
                return ResultDTO.Failure(404, "Could not find logged in user");
            }

            var isFavorite = loggedInUser.FavoriteProfiles?.Contains(profileId) ?? false;

            if (!isFavorite || loggedInUser.FavoriteProfiles == null)
            {
                return ResultDTO.Failure(404, "Your list of favorites does not contain profile to be removed");
            }

            loggedInUser.FavoriteProfiles.Remove(profileId);

            var updateUser = await _accountRepository.SaveChangesAsync();

            if (!updateUser)
            {
                return ResultDTO.Failure(500, "Changes could not be saved to account due to internal server error");
            }

            return ResultDTO.Succes(loggedInUser!.FavoriteProfiles!, 204, "Profile has been removed from favorites");
        }

        /*
         * TODO: Consider to move this to AccountService instead
         */
        public async Task<ResultDTO> AddProfileToFavorites(Guid profileId, string userAccountId)
        {
            if (string.IsNullOrEmpty(profileId.ToString()) || string.IsNullOrEmpty(userAccountId))
            {
                return ResultDTO.Failure(400, "Invalid inputs");
            }

            // Check profile doesn't belong to logged in account
            var loggedInUser = await _accountRepository.GetByIdAsync(Guid.Parse(userAccountId));

            if (loggedInUser == null)
            {
                return ResultDTO.Failure(404, "Could not find logged in user");
            }

            var profileFound = loggedInUser.Profiles?.FirstOrDefault(x => x.ProfileId == profileId);

            if (profileFound != null)
            {
                return ResultDTO.Failure(409, "You cannot add profile that belongs to your own account");
            }

            // Check profile is not already added to favorites
            var isFavorite = loggedInUser.FavoriteProfiles?.Contains(profileId) ?? false;

            if (isFavorite)
            {
                return ResultDTO.Failure(409, "Your list of favorites already contains this profile");
            }

            if (loggedInUser.FavoriteProfiles == null)
            {
                loggedInUser.FavoriteProfiles = new List<Guid>() { profileId };
            }
            else
            {
                loggedInUser.FavoriteProfiles.Add(profileId);
            }

            var updateUser = await _accountRepository.SaveChangesAsync();

            if (!updateUser)
            {
                return ResultDTO.Failure(500, "Changes could not be saved to account due to internal server error");
            }

            return ResultDTO.Succes(loggedInUser!.FavoriteProfiles!, 200, "Profile has been added to favorites");
        }

        public async Task<ResultDTO> SearchQuery(ProfileSearchQueryInput dto)
        {
            if (dto == null)
            {
                return ResultDTO.Failure(400, "Invalid input");
            }

            var queriedProfiles = await _helper.QuerySearchedProfiles(dto);

            if (queriedProfiles == null)
            {
                return ResultDTO.Failure(500, "Failed to retrieve profiles due to internal server error");
            }

            return ResultDTO.Succes(queriedProfiles, 200, "Profiles have been retrieved");
        }

        public async Task<ResultDTO> DeleteProfileById(ProfileDeleteInput dto, string userAccountId)
        {
            if (string.IsNullOrEmpty(dto.ProfileId.ToString()) || string.IsNullOrEmpty(userAccountId))
            {
                return ResultDTO.Failure(400, "Invalid inputs");
            }

            // Check logged in user exist
            var loggedInUser = await _accountRepository.GetByIdAsync(Guid.Parse(userAccountId));

            if (loggedInUser == null)
            {
                return ResultDTO.Failure(404, "Account does not have any profiles");
            }

            // Check correct password has been provided
            var hashedPassword = Authentication.HashingPasswordWithSaltUsingSHA256(dto.Password, Guid.Parse(userAccountId));

            if(!loggedInUser.Password.Equals(hashedPassword))
            {
                return ResultDTO.Failure(403, "Invalid password");
            }

            // Check if user has particular profile
            var profileToDelete = loggedInUser.Profiles?.FirstOrDefault(x => x.ProfileId == dto.ProfileId);

            if (profileToDelete == null)
            {
                return ResultDTO.Failure(403, $"Your account doesn't contain profile with profileId {dto.ProfileId}");
            }

            var deleted = await _profileRepository.DeleteProfileAsync(Guid.Parse(userAccountId), profileToDelete);

            if (!deleted)
            {
                return ResultDTO.Failure(500, $"Profile {dto.ProfileId} wasn't deleted due to internal server error");
            }

            // TODO: Remember to delete images from cloud

            return ResultDTO.Succes(deleted, 204, "Profile has been deleted");
        }


        public async Task<ResultDTO> UpdateProfile(ProfileUpdateInput dto, string userAccountId)
        {
            if (dto == null || string.IsNullOrEmpty(userAccountId))
            {
                return ResultDTO.Failure(400, "Invalid inputs");
            }

            // Check logged in user exist
            var loggedInUser = await _accountRepository.GetByIdAsync(Guid.Parse(userAccountId));

            if (loggedInUser == null)
            {
                return ResultDTO.Failure(404, "Account has no profiles");
            }

            // Check correct password has been provided
            var hashedPassword = Authentication.HashingPasswordWithSaltUsingSHA256(dto.Password, Guid.Parse(userAccountId));

            if (!loggedInUser.Password.Equals(hashedPassword))
            {
                return ResultDTO.Failure(403, "Invalid password");
            }

            // Check if user has authorization
            var profileFound = await _profileRepository.GetByIdAsync(dto.ProfileId);

            if(profileFound == null)
            {
                return ResultDTO.Failure(404, "Profile not found");
            }

            var hasAutorization = profileFound.AccountId.Equals(Guid.Parse(userAccountId)); 

            if (!hasAutorization)
            {
                return ResultDTO.Failure(403, "You don't have authorization to update this profile");
            }

            // Update profile
            var updatedProfile = await _profileRepository.UpdateAsync(dto);

            if (updatedProfile == null)
            {
                return ResultDTO.Failure(500, $"Failed to update profile {dto.ProfileId} due to internal server error");
            }

            // TODO: if changes has been made to profilePicture or later portfolio make sure to update those as well in the cloud

            return ResultDTO.Succes(updatedProfile, 200, "Your profile has been updated");
        }

        public async Task<ResultDTO> GetProfileById(Guid profileId)
        {
            if (string.IsNullOrEmpty(profileId.ToString()))
            {
                return ResultDTO.Failure(400, "Invalid input");
            }

            var profile = await _profileRepository.GetByIdAsync(profileId);

            if (profile == null)
            {
                return ResultDTO.Failure(404, "Profile was not found");
            }

            return ResultDTO.Succes(profile, 200, "Profile was found");
        }


        public async Task<ResultDTO> CreateProfile(ProfileCreateInput dto, string userAccountId)
        {
            if (dto == null || string.IsNullOrEmpty(userAccountId))
            {
                return ResultDTO.Failure(400, "Inputs are invalid");
            }

            // Check logged in user exist
            var loggedInUser = await _accountRepository.GetByIdAsync(Guid.Parse(userAccountId));

            if (loggedInUser == null)
            {
                return ResultDTO.Failure(404, "Logged in account was not found");
            }

            // Check AccountId in dto match userAccountId
            if (!dto.AccountId.ToString().Equals(userAccountId))
            {
                return ResultDTO.Failure(403, "You cannot create a profile for this account");
            }

            // Map dto to Profile
            var profile = ProfileMapper.MapToProfile(dto);

            // Create profile (composition)
            var createdProfile = await _profileRepository.CreateProfileAsync(Guid.Parse(userAccountId), profile);

            if (createdProfile == null)
            {
                return ResultDTO.Failure(500, $"Profile could not be created for account {userAccountId} due to internal server error");
            }


            // TODO: Handle ProfilePicture
            

            return ResultDTO.Succes(createdProfile, 201, "Profile has been created");
        }

        public async Task<ResultDTO> UploadProfilePicture(ProfileMediaUpload dto, string userAccountId)
        {
            if ( dto == null || string.IsNullOrEmpty(userAccountId) )
            {
                return ResultDTO.Failure(400, "Invalid inputs");
            }

            // Check if uploaded file exceeds 10 MB (maximum image file size for Cloudinary)
            if( dto.UploadFile.Length > 10 * 1024 * 1024 )
            {
                return ResultDTO.Failure(413, "File size can't exceed 10 MB");
            }

            // Check if uploaded file is limited to allowed content-types
            var allowedContentTypes = new[] { "image/jpeg", "image/png", "image/gif", "video/mp4", "video/mpeg" };

            if ( !allowedContentTypes.Contains(dto.UploadFile.ContentType) )
            {
                return ResultDTO.Failure(415, "Invalid content-type. Allowed content-type: JPG, PNG, GIF, MP4 and MPEG");
            }

            // User and profile

            var loggedInUser = await _accountRepository.GetByIdAsync(Guid.Parse(userAccountId));

            if(loggedInUser == null)
            {
                return ResultDTO.Failure(404, "Logged in account was not found");
            }

            // Check user's account contains profile with provided ProfileId
            var profile = loggedInUser.Profiles?.FirstOrDefault(x => x.ProfileId == dto.ProfileId);

            if (profile == null)
            {
                return ResultDTO.Failure(404, "Account does not contain Profile with specified ProfileId");
            }

            // If ProfilePicture is not null, remove media from Cloudinary and database
            if(!string.IsNullOrEmpty(profile.MediaId))
            {
                var mediaRemoved = await _helper.DeleteMedia(profile.MediaId, profile);

                if(!mediaRemoved)
                {
                    return ResultDTO.Failure(500, "System failed to remove existing profile-picture");
                }
            }

            // Upload result to Cloudinary

            if (dto.UploadFile == null || dto.UploadFile?.Length == 0)
            {
                return ResultDTO.Failure(400, "Invalid upload file");
            }

            // Save Media
            var mediaSaved = await _helper.SaveMedia(dto.UploadFile!, profile);

            if (mediaSaved == null)
            {
                return ResultDTO.Failure(500, "System failed to upload or failed save upload-result");
            }

            // Mapping to response-DTO
            var mediaOutput = new MediaOutput
            {
                MediaId = mediaSaved.MediaId,
                URL = mediaSaved.URL,
                ProfileId = profile.ProfileId,
            };

            return ResultDTO.Succes(mediaOutput, 201, "Profile has been created with profile picture");

        }

        public async Task<ResultDTO> RemoveProfilePicture(string mediaId, string userAccountId)
        {
            // null checks - 400
            if( string.IsNullOrEmpty(mediaId) || string.IsNullOrEmpty(userAccountId) )
            {
                return ResultDTO.Failure(400, "Invalid inputs");
            }

            // check logged in user exist - 404
            var loggedInUser = await _accountRepository.GetByIdAsync(Guid.Parse(userAccountId));

            if( loggedInUser == null)
            {
                return ResultDTO.Failure(404, "Logged in user was not found");
            }

            // check logged in user has any profiles - 409
            if( loggedInUser.Profiles == null || loggedInUser.Profiles.Count == 0 )
            {
                return ResultDTO.Failure(409, $"Logged in user has no profiles and can't possibly have any profile-picture with mediaId {mediaId}");
            }

            // check if any of logged in user's profile contain provided MediaId - 403
            var containsMedia = loggedInUser.Profiles.FirstOrDefault(x => x.MediaId == mediaId);

            if( containsMedia == null )
            {
                return ResultDTO.Failure(404, $"Profile-picture with mediaId {mediaId} was not found");
            }

            // remove from cloudinary and database - 500
            var mediaDeleted = await _helper.DeleteMedia(mediaId, containsMedia);

            if(!mediaDeleted)
            {
                return ResultDTO.Failure(500, $"Failed to remove profile-picture {mediaId}");
            }

            return ResultDTO.Succes(mediaDeleted, 204, $"Profile-picture {mediaId} has been removed from database and cloud");

        }
    }
}
