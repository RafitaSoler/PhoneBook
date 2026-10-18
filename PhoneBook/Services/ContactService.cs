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

        internal static Contact? Get(int id)
        {
            using var db = new PhoneBookDb();
            return db.Contacts.AsNoTracking().Where(c => c.Id == id).FirstOrDefault();
        }

        internal static int Add(string name, string phoneNumber, string? email)
        {
            using var db = new PhoneBookDb();
            Contact newContact = new() { Name = name, PhoneNumber = phoneNumber, Email = email};
            db.Contacts.Add(newContact);

            try
            {
                db.SaveChanges();
                return newContact.Id;
            }
            catch (DbUpdateException ex)
            {
                throw new ContactStorageException("The contact could not be added.", ex);
            }
        }

        internal static bool Update(int id, string? name, string? phoneNumber, string? email)
        {
            using var db = new PhoneBookDb();
            Contact? contact = db.Contacts.Find(id);
            if (contact == null) return false;

            contact.Name = name ?? contact.Name;
            contact.PhoneNumber = phoneNumber ?? contact.PhoneNumber;
            contact.Email = email;

            try
            {
                db.SaveChanges();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
            catch (DbUpdateException ex)
            {
                throw new ContactStorageException("The contact could not be saved.", ex);
            }
        }

        internal static bool Delete(int id)
        {
            using var db = new PhoneBookDb();
            Contact? contact = db.Contacts.Find(id);
            if (contact == null) return false;

            db.Contacts.Remove(contact);

            try
            {
                db.SaveChanges();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
            catch (DbUpdateException ex)
            {
                throw new ContactStorageException("The contact could not be deleted.", ex);
            }
        }
    }
}
