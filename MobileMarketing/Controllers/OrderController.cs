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
    public class OrderController : ApiControllerBase<Order>
    {
        private readonly IOrderRepository _repository;
        public OrderController(IOrderRepository repository)
        {
            _repository = repository;
        }

        [HttpPost]
        [Route("[action]")]
        [Authorize(Roles = "Create")]
        public async Task<IActionResult> Create([FromBody] Order order)
        {
            Log.Information($"{nameof(Create)}");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _repository.CreateAsync(order);
            return Ok(order);
        }

        [HttpGet]
        [Route("[action]{id}")]
        [Authorize(Roles = "GetById")]
        public async Task<IActionResult> GetById(Guid id)
        {
            Log.Information($"{nameof(GetById)} Id {id}");
            Order? order = await _repository.GetAsync(x => x.Id == id);
            if (order != null)
            {
                return Ok(order);
            }
            return NotFound(id + "is not found!");
        }

        [HttpGet]
        [Route("[action]")]
        [Authorize(Roles = "GetAll")]
        public async Task<IActionResult> GetAll()
        {
            Log.Information($"{nameof(GetAll)}");
            IQueryable<Order> orders = await _repository.GetAllAsync(x => true);
            return Ok(orders);
        }

        [HttpPut]
        [Route("[action]")]
        [Authorize(Roles = "Update")]
        public async Task<IActionResult> Update([FromBody] Order order)
        {
            Log.Information($"{nameof(Update)} {order.Id}");
            if (ModelState.IsValid)
            {
                Order? updated = await _repository.UpdateAsync(order);
                if (updated != null)
                {
                    return Ok(updated);
                }
            }
            return BadRequest(order + "is not found");
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
