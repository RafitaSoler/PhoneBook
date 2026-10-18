using PhoneBook.Models;
using PhoneBook.Services;
using Spectre.Console;
using System.Collections;

namespace PhoneBook
{
    internal static class UIController
    {
        internal enum MainMenuOption
        {
            ViewContacts,
            AddContact,
            UpdateContact,
            DeleteContact,
            ExitApplication
        }

        private static readonly int CANCEL_OPTION = -1;

        internal static void MainMenu()
        {
            bool exitApp = false;
            while(!exitApp)
            {
                AnsiConsole.Clear();
                MainMenuOption option = AnsiConsole.Prompt(
                    new SelectionPrompt<MainMenuOption>()
                    .Title("Contacts")
                    .HighlightStyle(Style.Parse("cyan"))
                    .AddChoices(Enum.GetValues<MainMenuOption>())
                    .UseConverter(option => option switch
                    {
                        MainMenuOption.ViewContacts => "View Contacts",
                        MainMenuOption.AddContact => "Add Contact",
                        MainMenuOption.UpdateContact => "Update Contact",
                        MainMenuOption.DeleteContact => "Delete Contact",
                        MainMenuOption.ExitApplication => "Exit",
                        _ => option.ToString()
                    })
                    .WrapAround()
                );

                switch(option)
                {
                    case MainMenuOption.ViewContacts:
                        ViewContacts();
                        break;
                    case MainMenuOption.AddContact:
                        AddContact();
                        break;
                    case MainMenuOption.UpdateContact:
                        UpdateContact();
                        break;
                    case MainMenuOption.DeleteContact:
                        DeleteContact();
                        break;
                    case MainMenuOption.ExitApplication:
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
                table.AddRow(fakeId.ToString(), contact.Name, contact.Email ?? "", contact.PhoneNumber);
                fakeId++;
            }
            AnsiConsole.Clear();
            AnsiConsole.Write(table);
            PromptGoBack();
        }

        private static void AddContact()
        {
            string name = PromptName();

            string phone = PromptPhone();

            string? email = PromptEmail();
            if(string.IsNullOrEmpty(email)) email = null;

            try
            {
                ContactService.Add(name, phone, email);
                AnsiConsole.MarkupLine("[lime]New contact inserted[/]");
            }
            catch (ContactStorageException e)
            {
                AnsiConsole.MarkupLine($"[red]{e.Message}[/]");
            }
            PromptGoBack();
        }

        private static void UpdateContact()
        {
            Contact contact = PromptContactSelection("Select a contact to update");
            if (contact.Id != CANCEL_OPTION)
            {
                string? name = null;
                if (AnsiConsole.Confirm("Do you want to change the name?"))
                {
                    AnsiConsole.MarkupLine($"Previous name: [cyan]{Markup.Escape(contact.Name)}[/]");
                    name = PromptName();
                }

                string? phone = null;
                if (AnsiConsole.Confirm("Do you want to change the phone?"))
                {
                    AnsiConsole.MarkupLine($"Previous phone: [cyan]{contact.PhoneNumber}[/]");
                    phone = PromptPhone();
                }

                string? email = null;
                if (AnsiConsole.Confirm("Do you want to change the email?"))
                {
                    AnsiConsole.MarkupLine($"Previous email: [cyan]{Markup.Escape(contact.Email ?? "null")}[/]");
                    email = PromptEmail();
                }
                if (string.IsNullOrEmpty(email)) email = null;

                try
                {
                    ContactService.Update(contact.Id, name, phone, email);
                    AnsiConsole.MarkupLine("[lime]Contact updated[/]");
                }
                catch (ContactStorageException e)
                {
                    AnsiConsole.MarkupLine($"[red]{e.Message}[/]");
                }
                PromptGoBack();
            }
        }

        private static void DeleteContact()
        {
            Contact contact = PromptContactSelection("Select a contact to delete");
            if (contact.Id != CANCEL_OPTION)
            {
                if (AnsiConsole.Confirm($"Do you want to delete [cyan]{Markup.Escape(contact.Name.PadRight(2))} | {contact.PhoneNumber} | {Markup.Escape(contact.Email ?? "")}[/]?"))
                {
                    try
                    {
                        ContactService.Delete(contact.Id);
                        AnsiConsole.MarkupLine("[lime]Contact deleted[/]");
                    }
                    catch (ContactStorageException e)
                    {
                        AnsiConsole.MarkupLine($"[red]{e.Message}[/]");
                    }
                    PromptGoBack();
                }
            }
        }

        private static string PromptName()
        {
            string name = AnsiConsole.Prompt(
                    new TextPrompt<string>("Write the name of the contact")
                    .Validate(n => !string.IsNullOrWhiteSpace(n.Trim())
                        ? ValidationResult.Success()
                        : ValidationResult.Error("[red]The name can't be empty[/]")));
            return name.Trim();
        }

        private static string PromptPhone()
        {
            string phone = AnsiConsole.Prompt(
                    new TextPrompt<string>("Write the phone number in XXX-XXXX format (like 555-0123)")
                    .Validate(p => ContactValidator.IsValidPhone(p.Trim())
                        ? ValidationResult.Success()
                        : ValidationResult.Error("[red]Use the format 555-0123[/]")));
            return phone.Trim();
        }

        private static string PromptEmail()
        {
            string email = AnsiConsole.Prompt(
                new TextPrompt<string>("Write the email")
                .AllowEmpty()
                .Validate(e => string.IsNullOrWhiteSpace(e.Trim()) || ContactValidator.IsValidEmail(e.Trim())
                    ? ValidationResult.Success()
                    : ValidationResult.Error("[red]Please insert a valid email[/]")));
            return email.Trim();
        }

        private static Contact PromptContactSelection(string message)
        {
            List<Contact> contacts = ContactService.GetAll();
            Contact cancelOption = new() { Id = CANCEL_OPTION };
            contacts.Add(cancelOption);
            int maxLength = contacts.Max(s => s.Name.Length);
            Contact contact = AnsiConsole.Prompt(
                new SelectionPrompt<Contact>()
                .Title(message)
                .HighlightStyle(Style.Parse("cyan"))
                .UseConverter(contact => contact.Id == CANCEL_OPTION
                ? "[grey]Cancel[/]"
                : $"{contacts.IndexOf(contact) + 1,-4} | {contact.Name.PadRight(maxLength + 2)} | {contact.PhoneNumber} | {contact.Email}")
                .AddChoices(contacts)
                .WrapAround()
            );
            contacts.Remove(cancelOption);
            return contact;
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
