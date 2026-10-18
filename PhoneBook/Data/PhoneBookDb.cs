using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PhoneBook.Models;

namespace PhoneBook.Data
{
    internal class PhoneBookDb : DbContext
    {
        public DbSet<Contact> Contacts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite("Data Source=phone-book.db;Foreign Keys=true");
            options.LogTo(
                message => File.AppendAllText("log.txt", message + Environment.NewLine),
                [DbLoggerCategory.Database.Command.Name],
                LogLevel.Information);
            options.EnableSensitiveDataLogging();
        }
    }
}
