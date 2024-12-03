using CloudinaryDotNet;
using LinkUp_REST_API.DTOs.Requests;
using LinkUp_REST_API.DTOs.Requests.Completed;
using LinkUp_REST_API.Models;
using LinkUp_REST_API.Repositories.Interfaces.Completed;
using LinkUp_REST_API.Services.Interfaces;
using LinkUp_REST_API.Services.Interfaces.Completed;
using LinkUp_REST_API.Services.Interfaces.Pending;
using LinkUp_REST_API.Util;
using LinkUp_REST_API.Util.Mapper;
using REST_API.Repositories.Interfaces;

namespace LinkUp_REST_API.Services
{
    public class PitchService : IPitchService
    {
        private IPitchRepository _pitchRepository;
        private IAccountRepository _accountRepository;
        private IProfileRepository _profileRepository;

        public PitchService(IAccountRepository accountRepository, IPitchRepository pitchRepository, IProfileRepository profileRepository)
        {
            _accountRepository = accountRepository;
            _pitchRepository = pitchRepository;
            _profileRepository = profileRepository;
        }


        public async Task<ResultDTO> DeletePitchById(Guid pitchId, string userAccountId)
        {
            // null checks
            if( string.IsNullOrEmpty(userAccountId) || string.IsNullOrEmpty(pitchId.ToString()) )
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
                return ResultDTO.Failure(409, "User has no profiles at current moment and is unable to retrieve any associated pitches send from this account");
            }

            // check pitch exist
            var pitchFound = await _pitchRepository.GetByIdAsync(pitchId);

            if(pitchFound == null )
            {
                return ResultDTO.Failure(404, "Pitch was not found");
            }

            // check if logged in user it the sendingProfile (has authorization)
            var profileIds = loggedInUser.Profiles.Select(p => p.ProfileId).ToList();

            var isSendingProfile = false;

            foreach (var profileId in profileIds)
            {
                if (profileId == pitchFound.ProfileId)
                {
                    isSendingProfile = true;
                    break;
                }
            }

            if (!isSendingProfile)
            {
                return ResultDTO.Failure(403, "None of your profiles have send this pitch");
            }

            // delete pitch
            var pitchDeleted = await _profileRepository.DeletePitchAsync(pitchFound.ProfileId, pitchFound);

            if(!pitchDeleted)
            {
                return ResultDTO.Failure(500, "Pitch wasn't deleted due to internal server error");
            }

            return ResultDTO.Succes(pitchDeleted, 204, $"Pitch {pitchId} has been deleted");
        }

        public async Task<ResultDTO> GetPitchById(Guid pitchId, string userAccountId)
        {
            // null-checks
            if( string.IsNullOrEmpty(userAccountId) || string.IsNullOrEmpty(pitchId.ToString()))
            {
                return ResultDTO.Failure(400, "Invalid inputs");
            }

            var loggedInUser = await _accountRepository.GetByIdAsync(Guid.Parse(userAccountId));

            if(loggedInUser == null)
            {
                return ResultDTO.Failure(404, "Logged in user not found");
            }

            if(loggedInUser.Profiles == null || loggedInUser.Profiles.Count == 0)
            {
                return ResultDTO.Failure(409, "User has no profiles at current moment and is unable to retrieve any associated pitches");
            }

            // check pitch exist
            var pitchFound = await _pitchRepository.GetByIdAsync(pitchId);

            if( pitchFound == null)
            {
                return ResultDTO.Failure(404, "Pitch was not found");
            }

            // check if logged in user is associated to the pitch (has authorization)
            var profileIds = loggedInUser.Profiles.Select(p => p.ProfileId).ToList();
            var isAssociated = false;

            foreach (var profileId in profileIds)
            {
                if (profileId == pitchFound.ProfileId || profileId == pitchFound.RecipientProfileId)
                {
                    isAssociated = true;
                    break;
                }
            }

            if(!isAssociated)
            {
                return ResultDTO.Failure(403, "You are not associated with the pitch");
            }

            return ResultDTO.Succes(pitchFound, 200, $"Pitch {pitchId} has been retrieved");
        }


        public async Task<ResultDTO> GetAllAssociatedPithes(string userAccountId)
        {
            // null-check
            if (string.IsNullOrEmpty(userAccountId))
            {
                return ResultDTO.Failure(400, "Invalid input");
            }

            // get all profiles from logged in user
            var loggedInUser = await _accountRepository.GetByIdAsync(Guid.Parse(userAccountId));

            if (loggedInUser == null)
            {
                return ResultDTO.Failure(404, "Logged in user not found");
            }

            // get pitches send from user's profiles
            var allAssociatedPitches = new List<Pitch>();

            var sendPitches = await _pitchRepository.GetPitchesSendByAccount(loggedInUser);

            // get pitches received by user's profiles
            var receivedPitches = await _pitchRepository.GetPitchesReceivedByAccount(loggedInUser);

            // merge pitches
            allAssociatedPitches.AddRange(sendPitches ?? new List<Pitch>());
            allAssociatedPitches.AddRange(receivedPitches ?? new List<Pitch>());

            return ResultDTO.Succes(allAssociatedPitches, 200, "All associated profiles have been extracted");
        }


        public async Task<ResultDTO> CreatePitch(PitchCreateInput dto, string userAccountId)
        {
            // null-checks - 400
            if(dto == null || string.IsNullOrEmpty(userAccountId))
            {
                return ResultDTO.Failure(404, "Invalid inputs");
            }

            // get logged in account - 404
            var loggedInAccount = await _accountRepository.GetByIdAsync(Guid.Parse(userAccountId));

            if(loggedInAccount == null)
            {
                return ResultDTO.Failure(404, "Logged in user was not found");
            }

            // check account has at least one profile
            if(loggedInAccount.Profiles == null || loggedInAccount.Profiles.Count() == 0 )
            {
                return ResultDTO.Failure(409, "You must have at least one account before you are allowed to send a pitch");
            }

            // check account contains sendingProfile - 409
            var containSendingProfile = loggedInAccount.Profiles?.FirstOrDefault(x => x.ProfileId == dto.SenderProfileId);

            if(containSendingProfile == null)
            {
                return ResultDTO.Failure(409, "Logged in account doesn't contain sending-profile");
            }

            // check account doesn't contain recipientProfile - 409
            var containRecipietProfile = loggedInAccount.Profiles?.FirstOrDefault(x => x.ProfileId == dto.RecipientProfileId);

            if(containRecipietProfile != null)
            {
                return ResultDTO.Failure(409, "You cannot send a pitch to your own profiles");
            }

            // check recipientProfile exist - 404
            var recipientExist = await _profileRepository.GetByIdAsync(dto.RecipientProfileId);

            if(recipientExist == null)
            {
                return ResultDTO.Failure(404, "Recipient profile was not found");
            }

            // map to pitch 
            var pitch = PitchMapper.MapToPitch(dto);

            // create pitch
            var pitchCreated = await _profileRepository.CreatePitchAsync(dto.SenderProfileId, pitch);

            if(pitchCreated == null)
            {
                return ResultDTO.Failure(500, "Failed to create pitch due to internal server error");
            }

            return ResultDTO.Succes(pitchCreated, 201, "Pitch was created");
        }



    }
}
