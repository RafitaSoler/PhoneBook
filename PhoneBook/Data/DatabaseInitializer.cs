using PhoneBook.Models;

namespace PhoneBook.Data
{
    internal static class DatabaseInitializer
    {
        internal static void Initialize()
        {
            using var db = new PhoneBookDb();
            bool alreadyCreated = db.Database.EnsureCreated();
            if (!alreadyCreated) return;

            db.Contacts.AddRange(
                new Contact { Name = "Joan", PhoneNumber = "555-0123", Email = "joan@gmail.com" },
                new Contact { Name = "Ana", PhoneNumber = "555-0456", Email = "ana@hotmail.com"},
                new Contact { Name = "Marc", PhoneNumber = "555-0789", Email = "marc@yahoo.com"});
            db.SaveChanges();
        }
    }
}
