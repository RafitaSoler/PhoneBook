using System.Net.Mail;
using System.Text.RegularExpressions;

namespace PhoneBook
{
    internal static partial class ContactValidator
    {

        [GeneratedRegex(@"^[0-9]{3}-[0-9]{4}\z")]
        private static partial Regex PhoneRegex();

        public static bool IsValidPhone(string? phone)
        {
            return phone != null && PhoneRegex().IsMatch(phone);
        }

        public static bool IsValidEmail(string? email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return false;
            }

            if (!MailAddress.TryCreate(email, out var address))
            {
                return false;
            }

            return address.Address == email && address.Host.Contains('.');
        }
    }
}
