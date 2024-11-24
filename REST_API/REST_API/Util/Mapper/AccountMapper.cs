using REST_API.DTOs.AccountDomain;
using REST_API.Models;

namespace REST_API.Util.Mapper
{
    public class AccountMapper
    {
        // CreateAccountDTO -> Account
        public static Account MapToAccount(CreateAccountDTO createAccountDto)
        {
            return new Account
            {
                AccountId = Guid.NewGuid(),
                Email = createAccountDto.Email,
                Password = createAccountDto.Password, // TODO: Make sure to hash the password in real scenarios
                PersonInformation = new PersonInformation
                {
                    PersonInformationId = Guid.NewGuid(),
                    FirstName = createAccountDto.FirstName,
                    Surname = createAccountDto.Surname,
                    Phone = createAccountDto.Phone,
                    BirthDate = DateOnly.FromDateTime(createAccountDto.BirthDate),
                    Gender = createAccountDto.Gender
                },
            };
        }

        // UpdateAccountDTO -> Account
        public static Account MapUpdateAccountDTOToAccount(UpdateAccountDTO updateAccountDto, Account existingAccount)
        {
            if (existingAccount == null || updateAccountDto == null)
            {
                throw new ArgumentNullException("Account or UpdateAccountDTO cannot be null");
            }

            if (!string.IsNullOrEmpty(updateAccountDto.Email))
            {
                existingAccount.Email = updateAccountDto.Email;
            }

            if (!string.IsNullOrEmpty(updateAccountDto.FirstName))
            {
                existingAccount.PersonInformation.FirstName = updateAccountDto.FirstName;
            }

            if (!string.IsNullOrEmpty(updateAccountDto.Surname))
            {
                existingAccount.PersonInformation.Surname = updateAccountDto.Surname;
            }

            if (!string.IsNullOrEmpty(updateAccountDto.Phone))
            {
                existingAccount.PersonInformation.Phone = updateAccountDto.Phone;
            }

            if (updateAccountDto.BirthDate.HasValue)
            {
                existingAccount.PersonInformation.BirthDate = DateOnly.FromDateTime(updateAccountDto.BirthDate.Value);
            }

            if (!string.IsNullOrEmpty(updateAccountDto.Gender))
            {
                existingAccount.PersonInformation.Gender = updateAccountDto.Gender;
            }

            if (!string.IsNullOrEmpty(updateAccountDto.Password))
            {
                existingAccount.Password = updateAccountDto.Password; // In a real scenario, hash the password here
            }

            return existingAccount;
        }

    }
}
