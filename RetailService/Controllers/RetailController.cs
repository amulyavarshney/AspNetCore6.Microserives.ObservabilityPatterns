using Microsoft.AspNetCore.Mvc;
using RetailService.Models;
using RetailService.Services;
using RetailService.ViewModels;

namespace RetailService.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class RetailController : ControllerBase
    {
        private readonly IRetailService _service;
        public RetailController(IRetailService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ItemViewModel>>> GetAsync()
        {
            return Ok(await _service.GetAsync());
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ItemViewModel>> GetByIdAsync(int id)
        {
            return Ok(await _service.GetByIdAsync(id));
        }

        [HttpPost]
        public async Task<ActionResult<ItemViewModel>> CreateAsync(ItemCreateViewModel item)
        {
            return Ok(await _service.CreateAsync(item));
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<ItemViewModel>> UpdateAsync(int id, ItemUpdateViewModel item)
        {
            return Ok(await _service.UpdateAsync(id, item));
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            await _service.DeleteAsync(id);
            return Ok();
        }
    }
}
