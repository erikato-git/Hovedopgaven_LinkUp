using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace LinkUp_REST_API.Util.ValidationDefitions
{
    public class NameAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value is string firstName)
            {
                // Check if the name only contains letters and starts with a capital letter
                var namePattern = @"^[A-Z][a-zA-Z]+$";  // Starts with uppercase letter, followed by alphabetic characters
                return Regex.IsMatch(firstName, namePattern);
            }
            return false;
        }

        public override string FormatErrorMessage(string name)
        {
            return $"The {name} must start with a capital letter and contain only alphabetic characters.";
        }
    }
}
