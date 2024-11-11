namespace REST_API.Util
{
    public static class ErrorMessages
    {

        // AccountService -> AccountController

        // Login Errors
        public const string AccountService_Login_401InvalidCredentials = "Invalid username or password.";

        // CreateAccount Errors
        public const string AccountService_CreateAccount_409InvalidEmail = "Cannot create account. Email already taken";

        // UpdateAccount Errors
        public const string AccountService_UpdateAccount_403CannotUpdateAnotherAccount = "You cannot update account details for another account";
        public const string AccountService_UpdateAccount_409UserChangeEmailToAnotherEmailThatAlreadyExist = "You cannot change your email to another email that already exist";

        // GetAccountById Errors
        public const string AccountService_GetAccountById_403UserTriesToAccessAnotherAccount = "You cannot access another account";


        // AccountRepository -> AccountService

        // FindAccountByEmailAndPassword
        public const string AccountRepository_FindAccountByEmailAndPassword_EmailAndPasswordDontMatch = "Email and password dont match";

    }
}
