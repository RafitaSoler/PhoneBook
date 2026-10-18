namespace PhoneBook.Services
{
    internal class ContactStorageException : Exception
    {
        public ContactStorageException(string message, Exception inner)
            : base(message, inner) { }
    }
}
