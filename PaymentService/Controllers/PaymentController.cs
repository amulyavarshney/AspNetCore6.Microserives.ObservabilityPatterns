using Microsoft.AspNetCore.Mvc;
using PaymentService.Models;
using PaymentService.Services;
using PaymentService.ViewModels;

namespace PaymentService.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _service;
        public PaymentController(IPaymentService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PaymentViewModel>>> GetAsync()
        {
            return Ok(await _service.GetAsync());
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<PaymentViewModel>> GetByIdAsync(int id)
        {
            return Ok(await _service.GetByIdAsync(id));
        }

        [HttpPost]
        public async Task<ActionResult<PaymentViewModel>> CreateAsync(PaymentCreateViewModel payment)
        {
            return Ok(await _service.CreateAsync(payment));
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<PaymentViewModel>> UpdateAsync(int id, PaymentStatus status)
        {
            return Ok(await _service.UpdateAsync(id, status));
        }
    }
}
