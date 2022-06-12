using BatchPayment.Repository;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace Bank.Repository
{
    public class Repo
    {
        private ApplicationContext _context;

        public Repo(ApplicationContext context)
        {
            _context = context; 
        }

        public async Task MakePayment(Guid sourceAccount, Guid destinationAccount, decimal amount)
        {
            var accounts = await _context.Accounts
                .Where(x => x.AccountNumber == sourceAccount || x.AccountNumber == destinationAccount).ToListAsync();

            var employerAccount = accounts.Where(a => a.AccountNumber == sourceAccount).First();
            var employeeAccount = accounts.Where(a => a.AccountNumber == destinationAccount).First();

            employeeAccount.Balance += amount;
            employerAccount.Balance -= amount;

            _context.SaveChanges();
        }
    }
}
