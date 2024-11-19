using REST_API.DTOs.AccountDomain;
using REST_API.DTOs.ProfileDomain;
using REST_API.Models;
using REST_API.Repositories;
using REST_API.Repositories.Interfaces;
using REST_API.Services.Helpers;
using REST_API.Services.Interfaces;
using REST_API.Util;

namespace REST_API.Services
{
    public class ProfileService : IProfileService
    {
        private IAccountRepository _accountRepository;
        private IProfileRepository _profileRepository;
        private IProfileServiceHelper _profileServiceHelper;

        public ProfileService(IAccountRepository accountRepository, IProfileRepository profileRepository, IProfileServiceHelper profileServiceHelper)
        {
            _accountRepository = accountRepository;
            _profileRepository = profileRepository;
            _profileServiceHelper = profileServiceHelper;
        }

        public async Task<ResultDTO> CreateProfile(CreateProfileDTO dto)
        {
            try
            {
                var loggedInAccount = await _profileServiceHelper.GetAccountFromLoginId();

                if (loggedInAccount != null)
                {
                    var generatedProfile = _profileServiceHelper.CreateProfileDTOToProfile(dto);
                    
                    if(generatedProfile != null)
                    {
                        var createdProfile = await _accountRepository.CreateProfileAsync(loggedInAccount, generatedProfile);

                        if (createdProfile != null)
                        {
                            return ResultDTO.SuccesResult(generatedProfile, "Profile was succesfully created");
                        }
                        else
                        {
                            return ResultDTO.FailureResult(ErrorMessages.ProfileSerivce_CreateProfile_FailedToCreateProfileDueToInternalServerError);
                        }
                    }
                    else
                    {
                        return ResultDTO.FailureResult(ErrorMessages.ProfileSerivce_CreateProfile_CouldNotGenerateProfileFromDto);
                    }
                }
                else
                {
                    return ResultDTO.FailureResult(ErrorMessages.ProfileSerivce_CreateProfile_CouldNotFindAccountForLoggedInUser);
                }
            }
            catch (Exception ex)
            {
                // TODO: logging(ex)
            }

            return ResultDTO.FailureResult("Create profile failed");
        }

        public async Task<ResultDTO> UpdateProfile(UpdateProfileDTO dto)
        {
            try
            {
                var hasAuthorization = _profileServiceHelper.CheckAccountIdMatchLoginId(dto.AccountId);

                if (hasAuthorization)
                {
                    var generatedProfile = _profileServiceHelper.UpdateProfileDTOToProfile(dto);

                    if (generatedProfile != null)
                    {
                        var updatedProfiled = await _profileRepository.UpdateAsync(generatedProfile);

                        if(updatedProfiled != null)
                        {
                            return ResultDTO.SuccesResult(updatedProfiled, "Profile has succesfully been updated");
                        }
                        else
                        {
                            return ResultDTO.FailureResult(ErrorMessages.ProfileSerivce_UpdateProfile_FailedToUpdateProfileDueToInternalServerError);
                        }
                    }
                    else
                    {
                        return ResultDTO.FailureResult(ErrorMessages.ProfileSerivce_UpdateProfile_CouldNotGenerateProfileFromDto);
                    }
                }
                else
                {
                    return ResultDTO.FailureResult(ErrorMessages.ProfileSerivce_UpdateProfile_YouCannotUpdateProfileForAnotherAccount);
                }

            }
            catch (Exception ex)
            {
                // TODO: logging(ex)
            }

            return ResultDTO.FailureResult("Update profile failed");
        }


        public async Task<ResultDTO> DeleteProfile(Guid profileId)
        {
            try
            {
                var loggedInAccount = await _profileServiceHelper.GetAccountFromLoginId();

                if(loggedInAccount != null)
                {
                    var profileDeleted = await _accountRepository.DeleteProfileAsync(loggedInAccount, profileId);

                    if(profileDeleted)
                    {
                        return ResultDTO.SuccesResult(profileDeleted, "Profile has succesfully been deleted");
                    }
                    else
                    {
                        return ResultDTO.FailureResult(ErrorMessages.ProfileSerivce_DeleteProfile_FailedToDeleteProfileDueToInternalServerError);
                    }
                }
                else
                {
                    return ResultDTO.FailureResult(ErrorMessages.ProfileSerivce_DeleteProfile_FailedToDeleteProfileDueToLoggedInAccountWasntFound);
                }
            }
            catch (Exception ex)
            {
                // TODO: logging(ex)
            }

            return ResultDTO.FailureResult("Delete profile failed");
        }

        public async Task<ResultDTO> GetProfileById(Guid profileId)
        {
            try
            {
                var loggedInAccount = await _profileServiceHelper.GetAccountFromLoginId();
                
                if(loggedInAccount != null)
                {
                    var profileFound = _profileServiceHelper.GetProfileFromAccount(loggedInAccount, profileId);

                    if(profileFound != null )
                    {
                        return ResultDTO.SuccesResult(profileFound, "Profile was found");
                    }
                    else
                    {
                        return ResultDTO.FailureResult(ErrorMessages.ProfileSerivce_GetProfile_YouDontHaveAProfileInYourAccountWithTheProvidedId);
                    }
                }else
                {
                    return ResultDTO.FailureResult(ErrorMessages.ProfileSerivce_GetProfile_SystemCouldntFindSignedInAccount);
                }

            }
            catch (Exception ex)
            {
                // TODO: logging(ex)
            }

            return ResultDTO.FailureResult("Get profile failed");
        }

        public async Task<ResultDTO> SearchQuery(SearchQueryDTO dto)
        {
            try
            {
                var queriedProfiles = await _profileServiceHelper.SearchProfiles(dto);

                if(queriedProfiles != null)
                {
                    return ResultDTO.SuccesResult(queriedProfiles, "Profiles succesfully queried");
                }
                else
                {
                    return ResultDTO.FailureResult(ErrorMessages.ProfileSerivce_SearchProfile_FailedToQueryProfilesDueToInternalServerError);
                }
            }
            catch (Exception ex)
            {
                // TODO: logging(ex)
            }

            return ResultDTO.FailureResult("Gettings profiles failed");
        }


        public async Task<ResultDTO> SaveProfile(Guid profileId)
        {
            try
            {
                var loggedInAccount = await _profileServiceHelper.GetAccountFromLoginId();

                if (loggedInAccount != null)
                {
                    var profileSaved = await _accountRepository.AddSavedProfileAsync(loggedInAccount,profileId);

                    if (profileSaved)
                    {
                        return ResultDTO.SuccesResult(true, "Profile has been saved to account succesfully");
                    }
                    else
                    {
                        return ResultDTO.FailureResult(ErrorMessages.ProfileSerivce_SearchProfile_FailedToQueryProfilesDueToInternalServerError);
                    }
                }
                else
                {
                    return ResultDTO.FailureResult(ErrorMessages.ProfileSerivce_SearchProfile_SystemCouldntFindSignedInAccount);
                }

            }
            catch (Exception ex)
            {
                // TODO: logging(ex)
            }

            return ResultDTO.FailureResult("Save profile failed");
        }

    }
}
