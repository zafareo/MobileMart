using Application.DTO.Permission;
using Application.DTO.RolePermission;
using Application.Interfaces;
using Application.Models;
using Domain.Models.IdentityEntites;
using Domain.Models.UserModels;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Data;

namespace MobileMarketing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolePermissionController : ApiControllerBase<RolePermission>
    {
        private readonly IRolePermissionRepository _repository;
        public RolePermissionController(IRolePermissionRepository repository)
        {
            _repository = repository;
        }

        [HttpPost]
        [Route("[action]")]
        [Authorize(Roles = "Create")]   
        public async Task<ActionResult<Response<RolePermissionGetDTO>>> Create([FromBody] RolePermissionCreateDTO permission)
        {
            Log.Information($"{nameof(Create)}");
            RolePermission? mappedRolePermission = _mapper.Map<RolePermission>(permission);
            //var validationRes = _validator.Validate(mappedRolePermission);
            if (ModelState.IsValid)
            {
                mappedRolePermission = await _repository.CreateAsync(mappedRolePermission);
                var res = _mapper.Map < RolePermissionGetDTO>(mappedRolePermission);
                return Ok(new Response<RolePermissionGetDTO>(res));
            }
            return BadRequest(new Response<object>(false, ModelState.ValidationState));
        }

        [HttpGet]
        [Route("[action]{id}")]
        [Authorize(Roles = "GetById")]
        public async Task<ActionResult<Response<RolePermissionGetDTO>>> GetById(Guid id)
        {
            Log.Information($"{nameof(GetById)} Id {id}");
            RolePermission? rolePermission = await _repository.GetAsync(x => x.Id == id);
            if (rolePermission != null)
            {
                RolePermissionGetDTO mappedPer = _mapper.Map<RolePermissionGetDTO>(rolePermission);
                return Ok(new Response<RolePermissionGetDTO>(mappedPer));
            }
            return NotFound(new Response<RolePermission>(false, id + "is not found!"));
        }

        [HttpGet]
        [Route("[action]")]
        [Authorize(Roles = "GetAll")]
        public async Task<ActionResult<Response<IEnumerable<RolePermissionGetDTO>>>> GetAll()
        {
            Log.Information($"{nameof(GetAll)}");
            IEnumerable<RolePermission> rolePermissions = await _repository.GetAllAsync(x => true);
            IEnumerable<RolePermissionGetDTO> MappedPer = _mapper.Map<IEnumerable<RolePermissionGetDTO>>(rolePermissions);
            return Ok(new Response<IEnumerable<RolePermissionGetDTO>>(MappedPer));
        }

        [HttpPut]
        [Route("[action]")]
        [Authorize(Roles = "Update")]
        public async Task<ActionResult<Response<RolePermissionGetDTO>>> Update([FromBody] RolePermissionUpdateDTO permission)
        {
            Log.Information($"{nameof(Update)} {permission.RolePermissionId}");
            RolePermission? mappedPer = _mapper.Map<RolePermission>(permission);
            var validationResult = _validator.Validate(mappedPer);
            if (validationResult.IsValid)
            {
                mappedPer = await _repository.UpdateAsync(mappedPer);
                if (mappedPer != null)
                {
                    return Ok(new Response<RolePermissionGetDTO>(_mapper.Map<RolePermissionGetDTO>(mappedPer)));
                }
            }
            return BadRequest(new Response<RolePermission>(false, permission + "is not found"));
        }

        [HttpDelete]
        [Route("[action]{id}")]
        [Authorize(Roles = "Delete")]
        public async Task<ActionResult<Response<bool>>> Delete(Guid id)
        {
            Log.Information($"{nameof(Delete)}{id} deleted");
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
