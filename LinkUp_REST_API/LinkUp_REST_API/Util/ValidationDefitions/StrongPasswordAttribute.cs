using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace LinkUp_REST_API.Util.ValidationDefitions
{
    public class StrongPasswordAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value is string password)
            {
                // Regular expression for a strong password: at least one uppercase, one lowercase, one digit, and one special character
                var passwordPattern = @"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[\W_]).{8,}$";
                return Regex.IsMatch(password, passwordPattern);
            }
            return false;
        }

        public override string FormatErrorMessage(string name)
        {
            return $"The {name} must be at least 8 characters long and include at least one uppercase letter, one lowercase letter, one number, and one special character.";
        }
    }
}
