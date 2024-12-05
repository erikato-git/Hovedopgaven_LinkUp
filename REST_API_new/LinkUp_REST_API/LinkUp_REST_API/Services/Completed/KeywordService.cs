using LinkUp_REST_API.DTOs.Requests;
using LinkUp_REST_API.Repositories.Interfaces;
using LinkUp_REST_API.Repositories.Interfaces.Completed;
using LinkUp_REST_API.Services.Interfaces;
using LinkUp_REST_API.Util;
using LinkUp_REST_API.Util.Mapper;
using Microsoft.AspNetCore.Http.HttpResults;

namespace LinkUp_REST_API.Services.Completed
{
    public class KeywordService : IKeywordService
    {
        private IAccountRepository _accountRepository;
        private IProfileRepository _profileRepository;
        private IKeywordRepository _keywordRepository;

        public KeywordService(IAccountRepository accountRepository, IKeywordRepository keywordRepository, IProfileRepository profileRepository)
        {
            _accountRepository = accountRepository;
            _keywordRepository = keywordRepository;
            _profileRepository = profileRepository;
        }

        public async Task<ResultDTO> DeleteKeywordById(Guid keywordId, string userAccountId)
        {
            // null checks
            if (string.IsNullOrEmpty(keywordId.ToString()) || string.IsNullOrEmpty(userAccountId))
            {
                return ResultDTO.Failure(400, "Invalid inputs");
            }

            // check logged in user exist - 404
            var loggedInUser = await _accountRepository.GetByIdAsync(Guid.Parse(userAccountId));

            if (loggedInUser == null)
            {
                return ResultDTO.Failure(404, "Logged in user was not found");
            }

            // check if logged in user has any profiles - 409
            if (loggedInUser.Profiles == null || loggedInUser.Profiles.Count == 0)
            {
                return ResultDTO.Failure(409, "The logged-in user does not have any profiles associated, why it can't have any keyword associated to be deleted.");
            }

            // check if keyword exist - 404
            var keywordExist = await _keywordRepository.GetByIdAsync(keywordId);

            if (keywordExist == null)
            {
                return ResultDTO.Failure(404, $"No Keyword with particular id {keywordId} was found");
            }

            // profiles for logged in account contains this keyword - 403
            var profileFound = loggedInUser.Profiles.FirstOrDefault(x => x.KeywordId == keywordId);

            if (profileFound == null)
            {
                return ResultDTO.Failure(403, $"You don't have authorization to delete keyword with this Id: {keywordId}");
            }

            // delete keyword (composition)
            var keywordDeleted = await _profileRepository.DeleteKeywordAsync(profileFound.ProfileId, keywordExist);

            if (!keywordDeleted)
            {
                return ResultDTO.Failure(500, "Could not delete keyword due to internal server error");
            }

            return ResultDTO.Succes(keywordDeleted, 204, "Keyword was deleted");
        }


        public async Task<ResultDTO> UpdateKeyword(KeywordUpdateInput updateDto, string userAccountId)
        {
            // null checks
            if (updateDto == null || string.IsNullOrEmpty(userAccountId))
            {
                return ResultDTO.Failure(400, "Invalid inputs");
            }

            // get logged in user
            var loggedInUser = await _accountRepository.GetByIdAsync(Guid.Parse(userAccountId));

            if (loggedInUser == null)
            {
                return ResultDTO.Failure(404, "Logged in user was not found");
            }

            if (loggedInUser.Profiles == null || loggedInUser.Profiles.Count == 0)
            {
                return ResultDTO.Failure(409, "The logged-in user does not have any profiles associated, why it can't have any keyword associated.");
            }

            // check if user has any profiles that match profileId in updateDTO
            var profileFound = loggedInUser.Profiles.FirstOrDefault(x => x.KeywordId == updateDto.KeywordId);

            if (profileFound == null)
            {
                return ResultDTO.Failure(403, "Your profile / profiles do not contain provided keyword-id");
            }

            // make sure keyword exist
            var keywordExist = await _keywordRepository.GetByIdAsync(updateDto.KeywordId);

            if (keywordExist == null)
            {
                return ResultDTO.Failure(404, $"Could not find Keyword {updateDto.KeywordId}");
            }

            // make update
            var keywordUpdated = await _keywordRepository.UpdateAsync(updateDto);

            if (keywordUpdated == null)
            {
                return ResultDTO.Failure(500, $"Failed to update keyword {keywordExist.KeywordId} due to internal Server error");
            }

            return ResultDTO.Succes(keywordUpdated, 200, "Keyword has been updated");
        }


        public async Task<ResultDTO> GetKeywordById(Guid keywordId, string userAccountId)
        {
            // null checks
            if (string.IsNullOrEmpty(keywordId.ToString()) || string.IsNullOrEmpty(userAccountId))
            {
                return ResultDTO.Failure(400, "Invalid inputs");
            }

            // check logged in user has an account with a profile that contains the keywordId
            var loggedInUser = await _accountRepository.GetByIdAsync(Guid.Parse(userAccountId));

            if (loggedInUser == null)
            {
                return ResultDTO.Failure(404, "Logged in user was not found");
            }

            if (loggedInUser.Profiles == null || loggedInUser.Profiles.Count() == 0)
            {
                return ResultDTO.Failure(409, "The logged-in user does not have any profiles associated, why it can't have any keyword associated.");
            }

            var profileFound = loggedInUser.Profiles.FirstOrDefault(x => x.KeywordId == keywordId);

            if (profileFound == null)
            {
                return ResultDTO.Failure(403, "Your profile / profiles do not contain provided keyword-id");
            }

            // get keyword
            var keyword = await _keywordRepository.GetByIdAsync(keywordId);

            if (keyword == null)
            {
                return ResultDTO.Failure(404, "Keyword was not found");
            }


            return ResultDTO.Succes(keyword, 200, $"Keyword {keywordId} has been retrieved");
        }



        public async Task<ResultDTO> CreateKeyword(KeywordCreateInput createDto, string userAccountId)
        {
            // null checks
            if (createDto == null || string.IsNullOrEmpty(userAccountId))
            {
                return ResultDTO.Failure(400, "Invalid inputs");
            }

            // check profileId belongs to logged in user
            var loggedInUser = await _accountRepository.GetByIdAsync(Guid.Parse(userAccountId));

            if (loggedInUser == null)
            {
                return ResultDTO.Failure(404, "Logged in user not found");
            }

            if (loggedInUser.Profiles == null || loggedInUser.Profiles.Count() == 0)
            {
                return ResultDTO.Failure(409, "The logged-in user does not have any profiles associated, preventing the creation of a keyword.");
            }

            var profileFound = loggedInUser.Profiles.FirstOrDefault(x => x.ProfileId == createDto.ProfileId);

            if (profileFound == null)
            {
                return ResultDTO.Failure(403, "Your account does not contain provided profile-id");
            }

            if (!string.IsNullOrEmpty(profileFound.KeywordId.ToString()))
            {
                return ResultDTO.Failure(409, $"You can only attach one Keyword item to the specified profile {createDto.ProfileId}");
            }

            // map dto to keyword
            var keyword = KeywordMapper.MapToKeyword(createDto);

            // create keyword
            var keywordCreated = await _profileRepository.CreateKeywordAsync(profileFound.ProfileId, keyword);

            if (keywordCreated == null)
            {
                return ResultDTO.Failure(500, $"Failed to create keyword for profile {profileFound.ProfileId} due to internal server error");
            }

            return ResultDTO.Succes(keywordCreated, 201, $"Keyword for profile {profileFound.ProfileId} was created");

        }


    }
}
