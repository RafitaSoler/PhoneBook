using PhoneBook.Models;
using PhoneBook.Services;
using Spectre.Console;
using System.Collections;

namespace PhoneBook
{
    internal static class UIController
    {
        internal static void MainMenu()
        {
            bool exitApp = false;
            while(!exitApp)
            {
                AnsiConsole.Clear();
                string option = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                    .Title("Contacts")
                    .HighlightStyle(Style.Parse("cyan"))
                    .AddChoices("View Contacts", "Exit")
                    .WrapAround()
                );

                switch(option)
                {
                    case "View Contacts":
                        ViewContacts();
                        break;
                    case "Exit":
                        exitApp = true;
                        break;
                    default:
                        break;
                }
            }
        }

        private static void ViewContacts()
        {
            List<Contact> contacts = ContactService.GetAll();

            Table table = new Table().AddColumns("Id", "Name", "Email", "Phone Number");
            int fakeId = 1;
            foreach (Contact contact in contacts)
            {
                table.AddRow(fakeId.ToString(), contact.Name, contact.Email, contact.PhoneNumber);
                fakeId++;
            }
            AnsiConsole.Clear();
            AnsiConsole.Write(table);
            PromptGoBack();
        }

        private static void PromptGoBack()
        {
            AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .HighlightStyle(Style.Parse("cyan"))
                .AddChoices("Go Back")
            );
        }
    }
}
