using BatchPayment.Repository;
using Microsoft.AspNetCore.Mvc;

namespace BatchPayment.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PaymentController : ControllerBase
    {
        private PaymentRepo _repo;
        private readonly ILogger<PaymentController> _logger;

        public PaymentController(ILogger<PaymentController> logger, PaymentRepo repo)
        {
            _logger = logger;
            _repo = repo;
        }

        [Route("pay")]
        [HttpGet]
        public async Task<IActionResult> PaySalary()
        {
            try
            {
                await _repo.MakeMonthlySalaryPayment();
                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}