using REST_API.DTOs.AccountDomain;
using REST_API.DTOs.ProfileDomain;
using REST_API.Models;
using REST_API.Repositories;
using REST_API.Repositories.Interfaces;
using REST_API.Services.Helpers;
using REST_API.Services.IHelpers;
using REST_API.Services.Interfaces;
using REST_API.Util;
using REST_API.Util.Mapper;

namespace REST_API.Services.Domains
{
    public class ProfileService : IProfileService
    {
        private IAccountRepository _accountRepository;
        private IProfileRepository _profileRepository;
        private IProfileServiceHelper _profileServiceHelper;
        private IAuthentication _authentication;

        public ProfileService(IAccountRepository accountRepository, IProfileRepository profileRepository, IProfileServiceHelper profileServiceHelper, IAuthentication authentication)
        {
            _accountRepository = accountRepository;
            _profileRepository = profileRepository;
            _profileServiceHelper = profileServiceHelper;
            _authentication = authentication;
        }

        public async Task<ResultDTO> CreateProfile(CreateProfileDTO dto, String userAccountId)
        {
            try
            {
                var loggedInAccount = await _accountRepository.GetAccountByIdAsync(Guid.Parse(userAccountId));

                if (loggedInAccount != null)
                {
                    var generatedProfile = ProfileMapper.MapCreateProfileDTOToProfile(dto);

                    if (generatedProfile != null)
                    {
                        var createdProfile = await _accountRepository.CreateProfileAsync(loggedInAccount, generatedProfile);

                        if (createdProfile != null)
                        {
                            // Cloudinary

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

        public async Task<ResultDTO> UpdateProfile(UpdateProfileDTO dto, String userAccountId)
        {
            try
            {
                var hasAuthorization = _authentication.CheckAccountIdMatchLoginId(dto.AccountId, userAccountId);

                // Check account exist with profile with ProfiledId from dto exist

                if (hasAuthorization)
                {
                    var parsedGuid = Guid.Parse(userAccountId);
                    var existingAccount = await _accountRepository.GetAccountByIdAsync(parsedGuid);

                    if (existingAccount == null)
                    {
                        return ResultDTO.FailureResult(ErrorMessages.ProfileSerivce_UpdateAccount_LoggedInAccountDoesNotExist);
                    }

                    var existingProfile = await _profileServiceHelper.GetProfileFromAccount(existingAccount, dto.AccountId);

                    if(existingProfile != null)
                    {
                        var generatedProfile = ProfileMapper.MapUpdateProfileDTOToProfile(dto,existingProfile);

                        if (generatedProfile != null)
                        {
                            var updatedProfiled = await _profileRepository.UpdateAsync(generatedProfile);

                            if (updatedProfiled != null)
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


        public async Task<ResultDTO> DeleteProfile(Guid profileId, String userAccountId)
        {
            try
            {
                var loggedInAccount = await _accountRepository.GetAccountByIdAsync(Guid.Parse(userAccountId));

                if (loggedInAccount != null)
                {
                    var profileDeleted = await _accountRepository.DeleteProfileAsync(loggedInAccount, profileId);

                    if (profileDeleted)
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

        public async Task<ResultDTO> GetProfileById(Guid profileId, String userAccountId)
        {
            try
            {
                var loggedInAccount = await _accountRepository.GetAccountByIdAsync(Guid.Parse(userAccountId));

                if (loggedInAccount != null)
                {
                    var profileFound = await _profileServiceHelper.GetProfileFromAccount(loggedInAccount, profileId);

                    if (profileFound != null)
                    {
                        return ResultDTO.SuccesResult(profileFound, "Profile was found");
                    }
                    else
                    {
                        return ResultDTO.FailureResult(ErrorMessages.ProfileSerivce_GetProfile_YouDontHaveAProfileInYourAccountWithTheProvidedId);
                    }
                }
                else
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

                if (queriedProfiles != null)
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


        public async Task<ResultDTO> SaveProfile(Guid profileId, String userAccountId)
        {
            try
            {
                var loggedInAccount = await _accountRepository.GetAccountByIdAsync(Guid.Parse(userAccountId));

                if (loggedInAccount != null)
                {
                    var profileSaved = await _accountRepository.AddSavedProfileAsync(loggedInAccount, profileId);

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
