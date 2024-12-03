using LinkUp_REST_API.DTOs.Requests;
using LinkUp_REST_API.DTOs.Requests.Completed;
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

        public Task<ResultDTO> DeletePitchById(Guid id, string userAccountId)
        {
            throw new NotImplementedException();
        }

        public Task<ResultDTO> GetAllAssociatedPithes(string userAccountId)
        {
            throw new NotImplementedException();
        }

        public Task<ResultDTO> GetPitchById(Guid id, string userAccountId)
        {
            throw new NotImplementedException();
        }

    }
}
