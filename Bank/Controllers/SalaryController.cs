using Bank.Repository;
using Entities;
using Microsoft.AspNetCore.Mvc;

namespace Bank.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SalaryController : ControllerBase
    {
        private Repo _repo;
        private readonly ILogger<SalaryController> _logger;

        public SalaryController(ILogger<SalaryController> logger, Repo repo)
        {
            _logger = logger;
            _repo = repo;
        }

        [Route("makepayment")]
        [HttpPut]
        public async Task<IActionResult> MakePayment(PaymentDTO payment)
        {
            //This is to mock the api responds in at least 2 minutes
            //await Task.Delay(120000);

            try
            {
                await _repo.MakePayment(payment.SourceAccount, payment.DestinationAccount, payment.Amount);
                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}