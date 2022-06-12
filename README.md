# BatchPaymentRepo
This repo is used to demonstrate how to make batch payment(100k payments) in a reasonable amount of time 

# Introduction
This was an interview question i got recently at a fintech company which i failed, I was asked how I could optimize a code that made a batch payment from a source account and to about 100k different destination accounts, the current code made the payment synchronously by looping through every account

# Solution
The solution I came up with is making this payments happen asynchronously and in batches
Below is the code that does all the work
<pre>
  <code>
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
                //Start a batch of the payments simultaneously. 
                var tasks = totalAccounts.Skip(position).Take(400).Select(a => CallBankAPIAndMakePayment(firstAccount, a, random.Next(1, 9))).ToList();
                
                //wait for all the above payments to complete before moving to the next batch
                await Task.WhenAll(tasks);
                
                //Update the number of item to be skipped
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
  </code>
</pre>

