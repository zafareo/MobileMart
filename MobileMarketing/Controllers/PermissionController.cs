using Application.DTO.Permission;
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

namespace MobileMarketing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionController : ApiControllerBase<Permission>
    {
        private readonly IPermissionRepository _repository;
        
        public PermissionController(IPermissionRepository repository)
        {
            _repository = repository;
        }

        [HttpPost]
        [Route("[action]")]
        [Authorize(Roles = "Create")]
        public async Task<ActionResult<Response<PermissionGetDTO>>> Create([FromBody] PermissionCreateDTO permission)
            {
            Log.Information($"{nameof(Create)} {permission.PermissionName}");
            
            Permission? mappedPermission = _mapper.Map<Permission>(permission);
            //var validationRes = _validator.Validate(mappedPermission);
            if (ModelState.IsValid)
            {
                mappedPermission = await _repository.CreateAsync(mappedPermission);
                var res = _mapper.Map<PermissionGetDTO>(mappedPermission);
                return Ok(new Response<PermissionGetDTO>(res));
            }
            return BadRequest(new Response<object>(false, ModelState.ValidationState));
        }

        [HttpGet]
        [Route("[action]{id}")]
        [Authorize(Roles = "GetById")]
        public async Task<ActionResult<Response<PermissionGetDTO>>> GetById(Guid id)
        {
            Log.Information($"{nameof(GetById)} Id {id}");
            Permission? permission = await _repository.GetAsync(x => x.Id == id);
            if (permission != null)
            {
                PermissionGetDTO mappedPer = _mapper.Map<PermissionGetDTO>(permission);
                return Ok(new Response<PermissionGetDTO>(mappedPer));
            }
            return NotFound(new Response<Permission>(false, id + "is not found!"));
        }

        [HttpGet]
        [Route("[action]")]
        [Authorize(Roles = "GetAll")]
        public async Task<ActionResult<Response<IEnumerable<PermissionGetDTO>>>> GetAll()
        {
            Log.Information($"{nameof(GetAll)}");
            IEnumerable<Permission> permissions = await _repository.GetAllAsync(x => true);
            IEnumerable<PermissionGetDTO> MappedPer = _mapper.Map<IEnumerable<PermissionGetDTO>>(permissions);
            return Ok(new Response<IEnumerable<PermissionGetDTO>>(MappedPer));
        }



        [HttpPut]
        [Route("[action]")]
        [Authorize(Roles = "Update")]
        public async Task<ActionResult<Response<PermissionGetDTO>>> Update([FromBody] PermissionUpdateDTO permission)
        {
            Log.Information($"{nameof(Update)} {permission.PermissionId}");
            Permission? mappedPer = _mapper.Map<Permission>(permission);
            var validationResult = _validator.Validate(mappedPer);
            if (validationResult.IsValid)
            {
                mappedPer = await _repository.UpdateAsync(mappedPer);
                if (mappedPer != null)
                {
                    return Ok(new Response<PermissionGetDTO>(_mapper.Map<PermissionGetDTO>(mappedPer)));
                }
            }
            return BadRequest(new Response<Permission>(false, permission + "is not found"));
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
