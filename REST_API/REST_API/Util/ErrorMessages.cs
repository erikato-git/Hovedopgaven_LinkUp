namespace REST_API.Util
{
    public static class ErrorMessages
    {
        // AccountService
        public const string AccountService_Login_InvalidEmailOrPassword = "Invalid email or password";        // generic error-message
        public const string AccountService_CreateAccount_EmailForAccountAlreadyExist = "Email for account already exist";        // helpful for users, also helpful for attackers
        public const string AccountSerivce_CreateAccount_CreateAccountFailed = "Create account failed";
        public const string AccountSerivce_UpdateAccount_YouCannotUpdateAnotherPersonsAccount = "You cannot update another person's account";
        public const string AccountSerivce_UpdateAccount_YouMustBeSignedInBeforeYouCanUpdateYourAccount = "You must be signed in before you can update your account";
        public const string AccountSerivce_UpdateAccount_UpdateAccountFailed = "Update account failed due to internal server failure";
        public const string AccountSerivce_UpdateAccount_LoggedInAccountDoesNotExist = "Logged in account does not exist";
        public const string AccountSerivce_GetAccountById_AccountNotFound = "Account not found";
        public const string AccountSerivce_GetAccountById_YouCannotAccessAnotherAccount = "You cannot access another account";
        public const string AccountSerivce_GetAccountById_FailedToRetrieveAccountInternalServerError = "Failed to retrieve account due to internal server failure";
        public const string AccountSerivce_GetAccountById_CannotRetrieveAnothersAccount = "You do not have permission to retrieve another person's account";
        public const string AccountSerivce_DeleteAccount_DeleteAccountFailed = "Delete account failed";
        public const string AccountSerivce_DeleteAccount_YouCannotDeleteAnotherAccount = "You cannot delete another account";
        public const string AccountSerivce_DeleteAccountById_CannotDeleteAnotherPersonsAccount = "You cannot delete another person's account";

        // ProfileService
        public const string ProfileSerivce_CreateProfile_FailedToCreateProfileDueToInternalServerError = "Failed to create profile due to internal server error";
        public const string ProfileSerivce_CreateProfile_FailedToCreateProfile = "Failed to create profile";
        public const string ProfileSerivce_CreateProfile_CouldNotGenerateProfileFromDto = "Could not generate profile from provided profile details";
        public const string ProfileSerivce_CreateProfile_CouldNotFindAccountForLoggedInUser = "Could not find account for signed in user";
        public const string ProfileSerivce_UpdateProfile_FailedToUpdateProfileDueToInternalServerError = "Failed to update profile due to internal server error";
        public const string ProfileSerivce_UpdateProfile_YouCannotUpdateProfileForAnotherAccount = "You cannot update a profile for another user's account";
        public const string ProfileSerivce_UpdateProfile_CouldNotGenerateProfileFromDto = "Could not generate profile from provided profile details";
        public const string ProfileSerivce_DeleteProfile_FailedToDeleteProfileDueToInternalServerError = "Failed to delete profile due to internal server error";
        public const string ProfileSerivce_DeleteProfile_FailedToDeleteProfileDueToLoggedInAccountWasntFound = "Failed to delete profile due to logged in account wasn't found";
        public const string ProfileSerivce_GetProfile_YouDontHaveAProfileInYourAccountWithTheProvidedId = "You don't have a profile in your account with the provided profile-id";
        public const string ProfileSerivce_GetProfile_SystemCouldntFindSignedInAccount = "System couldn't find signed in account";
        public const string ProfileSerivce_SearchProfile_FailedToQueryProfilesDueToInternalServerError = "Failed to query profiles due to internal server error";
        public const string ProfileSerivce_SearchProfile_SystemCouldntFindSignedInAccount = "System couldn't find signed in account";

        // PitchService
        public const string PitchSerivce_SendPitch_FailedToCreatePitchDueToInternalServerError = "Failed to create pitch due to internal server error";
        public const string PitchService_SendPitch_ReceipientsAccountDoesNotExist = "The account you try to send message to does not exist";
        public const string PitchService_SendPitch_YouAreNotAllowedToSendAnyPitchesBeforeYouHaveCreatedAtLeastOneProfile = "You are not allowed to send any pitches before you have created at least one profile";
        public const string PitchService_SendPitch_AccountForLoggedInUserWasNotFound = "Account for signed in user does not exist";
        public const string PitchService_IncomingPitches_FailedToFetchPitchesDueToInternalServerError = "Failed to fetch incoming pitches due to internal server error";
        public const string PitchService_IncomingPitches_AccountForSignedInUserWasNotFound = "Account for signed in user was not found";
        public const string PitchService_OutcomingPitches_FailedToFetchPitchesDueToInternalServerError = "Failed to outcoming fetch pitches due to internal server error";
        public const string PitchService_OutcomingPitches_AccountForSignedInUserWasNotFound = "Account for signed in user was not found";
        public const string PitchService_SaveProfile_ProfileFailedToBeAddedToAccountsListForSavedProfilesDueToInternalServerError = "Profile failed to be added to accounts list for saved profiles due to internale server error";
        public const string PitchService_SaveProfile_AccountForSignedInUserWasNotFound = "Account for signed in user was not found";




    }
}
