namespace PhoneBook.Tests
{
    public class ContactValidatorTests
    {
        [Theory]
        [InlineData("555-0123", true)]
        [InlineData("5550123", false)]
        [InlineData("55-50123", false)]
        [InlineData("555-012", false)]
        [InlineData("555-01234", false)]
        [InlineData("555-01a3", false)]
        [InlineData("555-0123\n", false)]
        [InlineData(" 555-0123", false)]
        [InlineData("", false)]
        [InlineData(null, false)]
        public void IsValidPhone_Works(string? input, bool expected)
        {
            Assert.Equal(expected, ContactValidator.IsValidPhone(input));
        }

        [Theory]
        [InlineData("joan@mail.com", true)]
        [InlineData("joan.smith@mail.co.uk", true)]
        [InlineData("joan@mail", false)]
        [InlineData("joan@", false)]
        [InlineData("@mail.com", false)]
        [InlineData("Joan <joan@mail.com>", false)]
        [InlineData("joan mail.com", false)]
        [InlineData("", false)]
        public void IsValidEmail_Works(string input, bool expected)
        {
            Assert.Equal(expected, ContactValidator.IsValidEmail(input));
        }
    }
}
