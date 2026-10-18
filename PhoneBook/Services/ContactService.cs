using Microsoft.EntityFrameworkCore;
using PhoneBook.Data;
using PhoneBook.Models;

namespace PhoneBook.Services
{
    internal static class ContactService
    {
        internal static List<Contact> GetAll()
        {
            using var db = new PhoneBookDb();
            return db.Contacts.AsNoTracking().ToList();
        }
    }
}
