using Entities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Polly;
using Polly.Retry;
using System.Text;

namespace BatchPayment.Repository
{
    public class PaymentRepo
    {
        private ApplicationContext _context;
        private readonly IHttpClientFactory _httpClientFactory;
        private HttpClient _httpClient;
        private readonly RetryPolicy _retryPolicy;
        private readonly string _apiURL = "https://localhost:7157";

        public PaymentRepo(ApplicationContext context, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _httpClientFactory = httpClientFactory;
            _retryPolicy = Policy.Handle<Exception>().Retry(2);
        }

        public async Task MakeMonthlySalaryPayment()
        {
            //Total number of accounts 100001 kept in an in memory data structure
            var totalAccounts = await _context.Accounts.AsNoTracking().ToListAsync();

            //Takes the first account which acts as the source account where money will be removed from
            var firstAccount = totalAccounts.First();

            //This is used to represent what amount of item to skip
            int position = 1;
            var totalCountOfAccounts = 100001;
            _httpClient = _httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri(_apiURL);

            //This is used to represent a number to be paid into an account
            var random = new Random();
            
            
            while (position < totalCountOfAccounts)
            {
                // Start a batch of the payment simultaneously.
                var tasks = totalAccounts.Skip(position).Take(400).Select(a => CallBankAPIAndMakePayment(firstAccount, a, random.Next(1, 9))).ToList();
                await Task.WhenAll(tasks);

                position += 400;
            }
        }


        private async Task CallBankAPIAndMakePayment(Account accountSource, Account accountDestination, decimal amount)
        {
            var uri = "/salary/makepayment";
            var paymentData = new PaymentDTO 
            { 
                SourceAccount = accountSource.AccountNumber,
                DestinationAccount = accountDestination.AccountNumber,
                Amount = amount
            };

            var json = JsonConvert.SerializeObject(paymentData);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            //Retries incase of an error
            await _retryPolicy.Execute(() => _httpClient.PutAsync(uri, data));

        }
    }
}
