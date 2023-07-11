using Application.Interfaces;
using Application.Models;
using Domain.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace MobileMarketing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderPhoneController : ApiControllerBase<Order>
    {
        private readonly IOrderPhoneRepository _repository;
        public OrderPhoneController(IOrderPhoneRepository repository)
        {
            _repository = repository;
        }

        [HttpPost]
        [Route("[action]")]
        [Authorize(Roles = "Create")]
        public async Task<IActionResult> Create([FromBody] OrderPhone orderPhone)
        {
            Log.Information($"{nameof(Create)}");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _repository.CreateAsync(orderPhone);
            return Ok(orderPhone);
        }

        [HttpGet]
        [Route("[action]{id}")]
        [Authorize(Roles = "GetById")]
        public async Task<IActionResult> GetById(Guid id)
        {
            Log.Information($"{nameof(GetById)} Id {id}");
            OrderPhone? orderPhone = await _repository.GetAsync(x => x.Id == id);
            if (orderPhone != null)
            {
                return Ok(orderPhone);
            }
            return NotFound(id + "is not found!");
        }

        [HttpGet]
        [Route("[action]")]
        [Authorize(Roles = "GetAll")]
        public async Task<IActionResult> GetAll()
        {
            Log.Information($"{nameof(GetAll)}");
            IQueryable<OrderPhone> orderPhones = await _repository.GetAllAsync(x => true);
            return Ok(orderPhones);
        }

        [HttpPut]
        [Route("[action]")]
        [Authorize(Roles = "Update")]
        public async Task<IActionResult> Update([FromBody] OrderPhone orderPhone)
        {
            Log.Information($"{nameof(Update)} {orderPhone.Id}");
            if (ModelState.IsValid)
            {
                OrderPhone? updated = await _repository.UpdateAsync(orderPhone);
                if (updated != null)
                {
                    return Ok(updated);
                }
            }
            return BadRequest(orderPhone + "is not found");
        }

        [HttpDelete]
        [Route("[action]{id}")]
        [Authorize(Roles = "Delete")]
        public async Task<IActionResult> Delete(Guid id)
        {
            Log.Information($"{nameof(Delete)}{id}");
            if (ModelState.IsValid)
            {
                bool IsSuccess = await _repository.DeleteAsync(id);
                if (IsSuccess)
                {
                    return Ok();
                }
            }
            return BadRequest($"failed to delete the {id}");
        }
    }
}
