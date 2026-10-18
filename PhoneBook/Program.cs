using PhoneBook.Data;

namespace PhoneBook
{
    internal class Program
    {
        static void Main(string[] args)
        {
            DatabaseInitializer.Initialize();
            UIController.MainMenu();
        }
    }
}