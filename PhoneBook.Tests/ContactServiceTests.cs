using PhoneBook.Data;
using PhoneBook.Models;
using PhoneBook.Services;

namespace PhoneBook.Tests
{
    public class ContactServiceTests
    {
        public ContactServiceTests()
        {
            DatabaseInitializer.Initialize();
        }

        [Fact]
        public void Add_ShouldCreateContact_Then_Get_ShouldReturnContact()
        {
            // Arrange
            string name = "OddMan";
            string phone = "555-1379";
            string email = "iloveoddnumbers@test.com";

            // Act
            int contactId = ContactService.Add(name, phone, email);
            Contact? contact = ContactService.Get(contactId);
            Contact? firstContact = ContactService.GetAll().FirstOrDefault();

            // Assert
            Assert.NotNull(contact);
            Assert.Equal(contactId, contact.Id);
            Assert.Equal(name, contact.Name);
            Assert.Equal(phone, contact.PhoneNumber);
            Assert.Equal(email, contact.Email);
            Assert.NotNull(firstContact);

            // Cleanup
            ContactService.Delete(contactId);
        }

        [Fact]
        public void Get_ShouldReturnNull_WhenContactDoesNotExist()
        {
            // Arrange
            int nonExistentId = -1;

            // Act
            Contact? contact = ContactService.Get(nonExistentId);

            // Assert
            Assert.Null(contact);
        }

        [Fact]
        public void Update_ShouldModifyContact()
        {
            // Arrange
            string name = "OddMan";
            string phone = "555-1379";
            string email = "iloveoddnumbers@test.com";
            int contactId = ContactService.Add(name, phone, email);

            string updatedName = "EvenMan";
            string updatedPhone = "555-2468";
            string updatedEmail = "iloveevennumbers@test.com";

            // Act
            ContactService.Update(contactId, updatedName, updatedPhone, updatedEmail);
            Contact? contact = ContactService.Get(contactId);

            // Assert
            Assert.NotNull(contact);
            Assert.Equal(updatedName, contact.Name);
            Assert.Equal(updatedPhone, contact.PhoneNumber);
            Assert.Equal(updatedEmail, contact.Email);

            // Cleanup
            ContactService.Delete(contactId);
        }

        [Fact]
        public void Delete_ShouldRemoveContact()
        {
            // Arrange
            string name = "OddMan";
            string phone = "555-1379";
            string email = "iloveoddnumbers@test.com";
            int contactId = ContactService.Add(name, phone, email);

            // Act
            ContactService.Delete(contactId);
            Contact? contact = ContactService.Get(contactId);

            // Assert
            Assert.Null(contact);
        }
    }
}
