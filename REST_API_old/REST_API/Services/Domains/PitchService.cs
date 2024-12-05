using REST_API.DTOs.PitchDomain;
using REST_API.Repositories.Interfaces;
using REST_API.Services.IHelpers;
using REST_API.Services.Interfaces;
using REST_API.Util;

namespace REST_API.Services.Domains
{
    public class PitchService : IPitchService
    {
        private IPitchRepository _pitchRepository;
        private IPitchServiceHelper _picthServiceHelper;
        private IAccountRepository _accountRepository;

        public PitchService(IPitchRepository pitchRepository, IPitchServiceHelper picthServiceHelper, IAccountRepository accountRepository)
        {
            _pitchRepository = pitchRepository;
            _picthServiceHelper = picthServiceHelper;
            _accountRepository = accountRepository;
        }

        public async Task<ResultDTO> SendPitch(SendPitchDTO dto, String userAccountId)
        {
            try
            {
                var loggedInUser = await _accountRepository.GetAccountByIdAsync(Guid.Parse(userAccountId));

                if (loggedInUser != null)
                {
                    if (loggedInUser?.Profiles?.Any() == false)
                    {
                        return ResultDTO.FailureResult(ErrorMessages.PitchService_SendPitch_YouAreNotAllowedToSendAnyPitchesBeforeYouHaveCreatedAtLeastOneProfile);
                    }

                    var receiverExist = await _picthServiceHelper.CheckReceiverExist(dto.RecipientAccountId);

                    if (receiverExist)
                    {
                        var pitch = _picthServiceHelper.SendPitchDTOToPitch(dto);

                        if (pitch != null)
                        {
                            var createdPitch = await _pitchRepository.AddAsync(pitch);

                            if (createdPitch != null)
                            {
                                return ResultDTO.SuccesResult(createdPitch, "Pitch has succesfully been created");
                            }
                            else
                            {
                                return ResultDTO.FailureResult(ErrorMessages.PitchSerivce_SendPitch_FailedToCreatePitchDueToInternalServerError);
                            }
                        }
                    }
                    else
                    {
                        return ResultDTO.FailureResult(ErrorMessages.PitchService_SendPitch_ReceipientsAccountDoesNotExist);
                    }
                }
                else
                {
                    return ResultDTO.FailureResult(ErrorMessages.PitchService_SendPitch_AccountForLoggedInUserWasNotFound);
                }
            }
            catch (Exception ex)
            {
                // TODO: logging(ex)
            }

            return ResultDTO.FailureResult("Send pitch failed");
        }

        public async Task<ResultDTO> GetIncomingPitches(String userAccountId)
        {
            try
            {
                var loggedInAccount = await _accountRepository.GetAccountByIdAsync(Guid.Parse(userAccountId));

                if (loggedInAccount != null)
                {
                    var incomingPitches = await _pitchRepository.GetPitchesByRecipientAccountIdAsync(loggedInAccount.AccountId);

                    if (incomingPitches != null)
                    {
                        return ResultDTO.SuccesResult(incomingPitches, "Pitches send to logged in account succesfully retrieved");
                    }
                    else
                    {
                        return ResultDTO.FailureResult(ErrorMessages.PitchService_IncomingPitches_FailedToFetchPitchesDueToInternalServerError);
                    }
                }
                else
                {
                    return ResultDTO.FailureResult(ErrorMessages.PitchService_IncomingPitches_AccountForSignedInUserWasNotFound);
                }
            }
            catch (Exception ex)
            {
                // TODO: logging(ex)
            }

            return ResultDTO.FailureResult("Failed to fecth incoming pitches");
        }

        public async Task<ResultDTO> GetOutcomingPitches(String userAccountId)
        {
            try
            {
                var loggedInAccount = await _accountRepository.GetAccountByIdAsync(Guid.Parse(userAccountId));

                if (loggedInAccount != null)
                {
                    var outcomingPitches = await _pitchRepository.GetPitchesByCreatorAsync(loggedInAccount.AccountId);

                    if (outcomingPitches != null)
                    {
                        return ResultDTO.SuccesResult(outcomingPitches, "Pitches send from logged in account succesfully retrieved");
                    }
                    else
                    {
                        return ResultDTO.FailureResult(ErrorMessages.PitchService_OutcomingPitches_FailedToFetchPitchesDueToInternalServerError);
                    }
                }
                else
                {
                    return ResultDTO.FailureResult(ErrorMessages.PitchService_OutcomingPitches_AccountForSignedInUserWasNotFound);
                }
            }
            catch (Exception ex)
            {
                // TODO: logging(ex)
            }

            return ResultDTO.FailureResult("Failed to fecth outcoming pitches");
        }

    }
}
