using Application.DTO.Permission;
using Application.DTO.Role;
using Application.DTO.User;
using Application.Interfaces;
using Application.Models;
using Domain.Models.UserModels;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Data;
using System.Security;

namespace MobileMarketing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class RoleController : ApiControllerBase<Role>
    {
        private readonly IRoleRepository _repository;
        public RoleController(IRoleRepository repository)
        {
            _repository = repository;
        }

        [HttpPost]
        [Route("[action]")]
        [Authorize(Roles = "Create")]
        public async Task<ActionResult<Response<RoleGetDTO>>> Create([FromBody] RoleCreateDTO role)
        {
            Log.Information($"{nameof(Create)} {role.Name}");
            Role? mappedRole = _mapper.Map<Role>(role);
            //var validateRes = _validator.Validate(mappedRole);
            if (ModelState.IsValid)
            {
                mappedRole = await _repository.CreateAsync(mappedRole);
                var res = _mapper.Map<RoleGetDTO>(mappedRole);
                return Ok(new Response<RoleGetDTO>(res));
            }
            return BadRequest(new Response<object>(false, ModelState.ValidationState));
        }

        [HttpGet]
        [Route("[action]{id}")]
        [Authorize(Roles = "GetById")]
        public async Task<ActionResult<Response<RoleGetDTO>>> GetById(Guid id)
        {
            Log.Information($"{nameof(GetById)}");
            Role? role = await _repository.GetAsync(x => x.Id == id);
            if (role != null)
            {
                RoleGetDTO mappedRole = _mapper.Map<RoleGetDTO>(role);
                return Ok(new Response<RoleGetDTO>(mappedRole));
            }
            return NotFound(new Response<Role>(false, id + "is not found!"));
        }

        [HttpGet]
        [Route("[action]")]
        [Authorize(Roles = "GetAll")]
        public async Task<ActionResult<Response<IQueryable<RoleGetDTO>>>> GetAll()
        {
            Log.Information($"{GetAll}");
            IEnumerable<Role> roles = await _repository.GetAllAsync(x => true);
            IEnumerable<RoleGetDTO> MappedRoles = _mapper.Map<IEnumerable<RoleGetDTO>>(roles);
            return Ok(new Response<IEnumerable<RoleGetDTO>>(MappedRoles));
        }

        [HttpPut]
        [Route("[action]")]
        [Authorize(Roles = "Update")]
        public async Task<ActionResult<Response<RoleGetDTO>>> Update([FromBody] RoleUpdateDTO role)
        {
            Log.Information($"{nameof(Update)}");
            Role? mappedRole = _mapper.Map<Role>(role);
            var validationResult = _validator.Validate(mappedRole);
            if (validationResult.IsValid)
            {
                mappedRole = await _repository.UpdateAsync(mappedRole);
                if (mappedRole != null)
                {
                    return Ok(new Response<RoleGetDTO>(_mapper.Map<RoleGetDTO>(mappedRole)));
                }
            }
            return BadRequest(new Response<Role>(false, role + "is not found"));
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
                    return Ok(new Response<bool>(true));
                }
            }
            return BadRequest(new Response<bool>(false, $"failed to delete the {id}"));
        }
    }
}
