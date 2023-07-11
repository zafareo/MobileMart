using Application.DTO.User;
using Application.DTO.UserRole;
using Application.Interfaces;
using Application.Models;
using Domain.Models.UserModels;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace MobileMarketing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class UserRoleController : ApiControllerBase<UserRole>
    {
        private readonly IUserRoleRepository _repository;
        public UserRoleController(IUserRoleRepository repository)
        {
            _repository = repository;
        }

        [HttpPost]
        [Route("[action]")]
        [Authorize(Roles = "Create")]
        public async Task<ActionResult<Response<UserRoleGetDTO>>> Create([FromBody] UserRoleCreateDTO userRole)
        {
            Log.Information($"created {userRole}");
            UserRole MappeduserRole = _mapper.Map<UserRole>(userRole);
            //var validationRes = _validator.Validate(MappeduserRole);
            if (!ModelState.IsValid)
            {
                return BadRequest(new Response<object>(false, ModelState.ValidationState));
            }
            MappeduserRole = await _repository.CreateAsync(MappeduserRole);
            var res = _mapper.Map<UserRoleGetDTO>(userRole);
            return Ok(new Response<UserRoleGetDTO>(res));
        }

        [HttpGet]
        [Route("[action]{id}")]
        [Authorize(Roles = "GetById")]
        public async Task<ActionResult<Response<UserRoleGetDTO>>> GetById(Guid id)
        {
            Log.Information($"UserRole Id {id}");
            UserRole? userRole = await _repository.GetAsync(x => x.Id == id);
            if (userRole != null)
            {
                UserRoleGetDTO mappedUser = _mapper.Map<UserRoleGetDTO>(userRole);
                return Ok(new Response<UserRoleGetDTO>(mappedUser));
            }
            return NotFound(new Response<UserRole>(false, id + "is not found!"));
        }

        [HttpGet]
        [Route("[action]")]
        [Authorize(Roles = "GetAll")]
        public async Task<ActionResult<Response<IQueryable<UserRoleGetDTO>>>> GetAll()
        {
            Log.Information($"{nameof(GetAll)}");
            IEnumerable<UserRole> userRoles = await _repository.GetAllAsync(x => true);
            IEnumerable<UserRoleGetDTO> mappedUserRoles = _mapper.Map<IEnumerable<UserRoleGetDTO>>(userRoles);
            return Ok(new Response<IEnumerable<UserRoleGetDTO>>(mappedUserRoles));

        }

        [HttpPut]
        [Route("[action]")]
        [Authorize(Roles = "Update")]
        public async Task<ActionResult<Response<UserRoleGetDTO>>> Update([FromBody] UserRoleUpdateDTO userRole)
        {
            Log.Information($"{nameof(Update)}");
            UserRole? mappedUser = _mapper.Map<UserRole>(userRole);
            var validatedUser = _validator.Validate(mappedUser);
            if (validatedUser.IsValid)
            {
                mappedUser = await _repository.UpdateAsync(mappedUser);
                if (mappedUser != null)
                {
                    return Ok(new Response<UserRoleGetDTO>(_mapper.Map<UserRoleGetDTO>(mappedUser)));
                }
            }
            return BadRequest(new Response<UserRole>(false, userRole + "is not found"));
        }

        [HttpDelete]
        [Route("[action]{id}")]
        [Authorize(Roles = "Delete")]
        public async Task<ActionResult<Response<bool>>> Delete(Guid id)
        {
            Log.Information($"{nameof(Delete)}");
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
