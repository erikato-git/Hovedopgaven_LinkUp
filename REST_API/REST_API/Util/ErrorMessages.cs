namespace REST_API.Util
{
    public static class ErrorMessages
    {
        public const string AccountService_InvalidEmailOrPassword = "Invalid email or password";        // generic error-message
        public const string AccountService_EmailForAccountAlreadyExist = "Email for account already exist";        // helpful for users, also helpful for attackers
        public const string AccountSerivce_YouCannotUpdateAnotherPersonsAccount = "You cannot update another person's account";
        public const string AccountSerivce_YouMustBeSignedInBeforeYouCanUpdateYourAccount = "You must be signed in before you can update your account";
    }
}
