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
    public class SubCategoryController : ApiControllerBase<SubCategory>
    {
        private readonly ISubCategoryRepository _repository;
        public SubCategoryController(ISubCategoryRepository repository)
        {
            _repository = repository;
        }

        [HttpPost]
        [Route("[action]")]
        [Authorize(Roles = "Create")]
        public async Task<IActionResult> Create([FromBody] SubCategory sub)
        {
            Log.Information($"{nameof(Create)}");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _repository.CreateAsync(sub);
            return Ok(sub);
        }

        [HttpGet]
        [Route("[action]{id}")]
        [Authorize(Roles = "GetById")]
        public async Task<IActionResult> GetById(Guid id)
        {
            Log.Information($"{nameof(GetById)} Id {id}");
            SubCategory? sub = await _repository.GetAsync(x => x.Id == id);
            if (sub != null)
            {
                return Ok(sub);
            }
            return NotFound(id + "is not found!");
        }

        [HttpGet]
        [Route("[action]")]
        [Authorize(Roles = "GetAll")]
        public async Task<IActionResult> GetAll()
        {
            Log.Information($"{nameof(GetAll)}");
            IQueryable<SubCategory> subCategories = await _repository.GetAllAsync(x => true);
            return Ok(subCategories);
        }

        [HttpPut]
        [Route("[action]")]
        [Authorize(Roles = "Update")]
        public async Task<IActionResult> Update([FromBody] SubCategory sub)
        {
            Log.Information($"{nameof(Update)} {sub.Id}");
            if (ModelState.IsValid)
            {
                SubCategory? updated = await _repository.UpdateAsync(sub);
                if (updated != null)
                {
                    return Ok(updated);
                }
            }
            return BadRequest(sub + "is not found");
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
