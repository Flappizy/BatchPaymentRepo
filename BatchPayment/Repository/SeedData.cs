using Entities;
using Microsoft.EntityFrameworkCore;

namespace BatchPayment.Repository
{
    public class SeedData
    {
        public static async Task EnsurePopulated(IApplicationBuilder app)
        {
            ApplicationContext context = app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<ApplicationContext>();
            var filePath = Directory.GetCurrentDirectory() + "\\Repository\\Names.txt";

            if (context.Database.GetPendingMigrations().Any())
            {
                context.Database.Migrate();
            }

            if (!context.Accounts.Any())
            {
                string[] names = File.ReadAllLines(filePath);
                var initials = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z"};
                var rnd = new Random();
                int randomIndex;  // creates a number between 1 and 10
                var employerAccount = new Account
                {
                    Name = "Ibukun B.",
                    AccountNumber = Guid.NewGuid(),
                    Balance = 5998833933
                };

                context.Accounts.Add(employerAccount);

                for (int i = 0; i < 100000; i++)
                {
                    randomIndex = rnd.Next(0, 26);
                    var employeeAccount = new Account
                    {
                        Name = names[randomIndex] + " " + initials[randomIndex] + ".",
                        AccountNumber = Guid.NewGuid(),
                        Balance = 0
                    };
                    context.Accounts.Add(employeeAccount);
                }

            }
            await context.SaveChangesAsync();
        }

    }
}
