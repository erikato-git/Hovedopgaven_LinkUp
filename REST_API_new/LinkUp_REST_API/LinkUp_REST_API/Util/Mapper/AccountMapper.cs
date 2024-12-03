using LinkUp_REST_API.Core;
using LinkUp_REST_API.DTOs.Requests.Completed;
using LinkUp_REST_API.DTOs.Responses.Completed;
using LinkUp_REST_API.Models;

namespace LinkUp_REST_API.Util.Mapper
{
    public class AccountMapper
    {
        public static LoginOutput MapToLoginOutput(Account account, string jwt)
        {
            if (account == null || account.PersonInformation == null || string.IsNullOrEmpty(jwt))
                throw new ArgumentNullException(nameof(account), "Account or JWT cannot be null");

            return new LoginOutput
            {
                AccountId = account.AccountId,
                Email = account.Email,
                FirstName = account.PersonInformation.FirstName,
                Surname = account.PersonInformation.Surname,
                Phone = account.PersonInformation.Phone,
                BirthDate = account.PersonInformation.BirthDate,
                Gender = account.PersonInformation.Gender,
                JWT = jwt,
                Profiles = account.Profiles,
            };
        }


        public static Account MapToAccount(AccountCreateInput input)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            var uniqueId = Guid.NewGuid();

            return new Account
            {
                AccountId = uniqueId, // Generate a new unique ID
                Email = input.Email,
                Password = Authentication.HashingPasswordWithSaltUsingSHA256(input.Password, uniqueId), // Optionally hash the password
                FavoriteProfiles = new List<Guid>(), // Initialize as empty
                PersonInformation = new PersonInformation
                {
                    PersonInformationId = Guid.NewGuid(), // Generate a unique ID for the associated PersonInformation
                    FirstName = input.PersonInformation.FirstName,
                    Surname = input.PersonInformation.Surname,
                    Phone = input.PersonInformation.Phone,
                    BirthDate = input.PersonInformation.BirthDate,
                    Gender = input.PersonInformation.Gender
                },
                Profiles = new List<Profile>() // Initialize as empty
            };
        }



        public static Account MapToUpdateAccount(AccountUpdateInput input, Account existingAccount)
        {
            if (input == null || existingAccount == null)
            {
                throw new ArgumentNullException(nameof(input), "Input or existing account cannot be null.");
            }

            // Update Account properties
            if (!string.IsNullOrEmpty(input.Email))
            {
                existingAccount.Email = input.Email;
            }

            if (!string.IsNullOrEmpty(input.Password))
            {
                existingAccount.Password = Authentication.HashingPasswordWithSaltUsingSHA256(input.Password, input.AccountId);
            }

            // Update PersonInformation properties
            var personInfo = existingAccount.PersonInformation;

            if (personInfo == null)
            {
                throw new InvalidOperationException("PersonInformation cannot be null in the existing account.");
            }

            if (!string.IsNullOrEmpty(input.FirstName))
            {
                personInfo.FirstName = input.FirstName;
            }

            if (!string.IsNullOrEmpty(input.Surname))
            {
                personInfo.Surname = input.Surname;
            }

            if (!string.IsNullOrEmpty(input.Phone))
            {
                personInfo.Phone = input.Phone;
            }

            if (input.BirthDate.HasValue)
            {
                personInfo.BirthDate = input.BirthDate.Value;
            }

            if (!string.IsNullOrEmpty(input.Gender))
            {
                personInfo.Gender = input.Gender;
            }

            return existingAccount;
        }


        public static AccountExternalDetailsOutput ToExternalDetailsOutput(Account account)
        {
            if (account == null)
            {
                throw new ArgumentNullException(nameof(account));
            }

            return new AccountExternalDetailsOutput
            {
                AccountId = account.AccountId,
                Email = account.Email,
                FirstName = account.PersonInformation.FirstName,
                Surname = account.PersonInformation.Surname,
                Phone = account.PersonInformation.Phone,
                BirthDate = account.PersonInformation.BirthDate,
                Gender = account.PersonInformation.Gender
            };
        }




        //

    }
}
