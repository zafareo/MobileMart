using Application.DTO.User;
using Application.Interfaces;
using Application.Models;
using Domain.Models.Entities;
using Domain.Models.Token;
using Domain.Models.UserModels;
using FluentValidation;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MobileMarketing.Filters;
using Serilog;
using System.Security.Claims;

namespace MobileMarketing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class UserController : ApiControllerBase<User>
    {
        private readonly IUserRepository _repository;
        private readonly IJWTService _jwtService;
        private readonly IUserRefreshTokenRepository _userRefreshTokenRepository;
        private readonly IConfiguration _configuration;
        public UserController(IUserRepository repository, IJWTService jwtService, IUserRefreshTokenRepository userRefreshTokenRepository, IConfiguration configuration)
        {
            _repository = repository;
            _jwtService = jwtService;
            _userRefreshTokenRepository = userRefreshTokenRepository;
            _configuration = configuration;
        }

        [HttpGet]
        [Route("[action]")]
        [AllowAnonymous]
        public async Task<ActionResult<Response<Pagination<User>>>> Search(string text, int page = 1, int pageSize = 10)
        {
            var Users = await _repository.GetAllAsync(x => x.UserName.ToString().Contains(text));

            Pagination<User> users = await Pagination<User>.CreateAsync(Users, page, pageSize);

            Response<Pagination<User>> res = new()
            {
                Result = users
            };
            return Ok(res);
        }


        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> RefreshToken(Token token)
        {
            Log.Information($"{nameof(RefreshToken)}");
            var principal = _jwtService.GetPrincipalFromExpiredToken(token.AccessToken);
            var name = principal.FindFirstValue(ClaimTypes.Name);
            var user = await _repository.GetAsync(x => x.UserName == name);
            var credential = new UserCredential
            {
                UserName = user.UserName,
                Password = user.Password
            };

            UserRefreshToken savedRefreshToken = await _userRefreshTokenRepository.GetSavedRefreshTokens(name, token.RefreshToken);
            if (savedRefreshToken == null || savedRefreshToken.RefreshToken != token.RefreshToken)
            {
                return Unauthorized("Invalid input");
            }
            if (savedRefreshToken.Expiretime < DateTime.UtcNow)
            {
                return Unauthorized(" time limit of the token has expired !");
            }

            var newJwt = await _jwtService.GenerateTokenAsync(user);
            if (newJwt == null)
            {
                return Unauthorized("Invalid input");
            }
            int min = 4;
            if (int.TryParse(_configuration["JWT:RefreshTokenExpiresTime"], out int _min))
            {
                min = _min;
            }
            UserRefreshToken refreshToken = new()
            {
                RefreshToken = newJwt.RefreshToken,
                UserName = name,
                Expiretime = DateTime.UtcNow.AddMinutes(min)
            };
            bool IsDeleted = await _userRefreshTokenRepository.DeleteUserRefreshTokens(name, token.RefreshToken);
            if (IsDeleted)
            {
                await _userRefreshTokenRepository.AddUserRefreshTokens(refreshToken);
            }
            else
            {
                return BadRequest();
            }
            return Ok(newJwt);

        }

        [HttpPost]
        [AllowAnonymous]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] UserCredential credential)
        {
            Log.Information("Login is called");
            string hashedPsw = await _repository.ComputeHashAsync(credential.Password);
            User? user = await _repository.GetAsync(x => x.UserName == credential.UserName && x.Password == hashedPsw);
            if (!await _userRefreshTokenRepository.IsValidUserAsync(user))
            {
                return Unauthorized();
            }
            int min = 4;
            if (int.TryParse(_configuration["JWT:RefreshTokenExpiresTime"], out int _min))
            {
                min = _min;
            }
            var token = await _jwtService.GenerateTokenAsync(user);
            var refreshToken = new UserRefreshToken
            {
                UserName = user.UserName,
                Expiretime = DateTime.UtcNow.AddMinutes(min),
                RefreshToken = token.RefreshToken
            };
            await _userRefreshTokenRepository.UpdateUserRefreshToken(refreshToken);
            return Ok(token);

        }

        [HttpPost]
        [Route("[action]")]
        [ActionModelValidation]
        [Authorize(Roles = "Create")]
        public async Task<ActionResult<Response<UserGetDTO>>> Create([FromBody] UserCreateDTO user)
        {
            Log.Information($"Created {user.UserName}");
            User Mappeduser = _mapper.Map<User>(user);
            //var validationRes = _validator.+Validate(Mappeduser);
            if (!ModelState.IsValid)
            {
                return BadRequest(new Response<object>(false, ModelState.ValidationState));
            }
            Mappeduser = await _repository.CreateAsync(Mappeduser);
            var res = _mapper.Map<UserGetDTO>(Mappeduser);
            return Ok(new Response<UserGetDTO>(res));   
            
        }

        [HttpGet]
        [Route("[action]{id}")]
        [Authorize(Roles = "GetById")]                                                                                                                                       
        public async Task<ActionResult<Response<UserGetDTO>>> GetById(Guid Id)
        {
            Log.Information($"User Id: {Id}");
            User? user = await _repository.GetAsync(x => x.Id == Id);
            if (user != null)
            {
                UserGetDTO mappedUser = _mapper.Map<UserGetDTO>(user);
                return Ok(new Response<UserGetDTO>(mappedUser));
            }
            return NotFound(new Response<User>(false, Id + "is not found!"));
        }

        [HttpGet]
        [Route("[action]")]
        [AllowAnonymous]
        [Authorize(Roles = "GetAllUsers")]
        public async Task<ActionResult<Response<Pagination<User>>>> GetAllUsers(int page = 1, int pageSize = 10)
        {
            IQueryable<User> Users = await _repository.GetAllAsync(x => true);

            Pagination<User> users = await Pagination<User>.CreateAsync(Users, page, pageSize);

            Response<Pagination<User>> res = new()
            {
                Result = users
            };
            return Ok(res);
        }

        [HttpGet]
        [Route("GetAll")]
        [Authorize(Roles ="GetAll")]
        public async Task<ActionResult<Response<IQueryable<UserGetDTO>>>> GetAll()
        {
            Log.Information($"{nameof(GetAll)}");
            IEnumerable<User> Users = await _repository.GetAllAsync(x=>true);
            IEnumerable<UserGetDTO> mappedUsers = _mapper.Map<IEnumerable<UserGetDTO>>(Users);
            return Ok(new Response<IEnumerable<UserGetDTO>>(mappedUsers));

        }


        [HttpPut]
        [Route("[action]")]
        [ActionModelValidation]
        [Authorize(Roles = "Update")]
        public async Task<ActionResult<Response<UserGetDTO>>> Update([FromBody] UserUpdateDTO user)
        {            
            Log.Information($" Updated : {user.UserName}");
            User? mappedUser = _mapper.Map<User>(user);
            var validatedUser = _validator.Validate(mappedUser);
            if (validatedUser.IsValid)
            {
                mappedUser = await _repository.UpdateAsync(mappedUser);
                if (mappedUser!=null)
                {
                    return Ok(new Response<UserGetDTO>(_mapper.Map<UserGetDTO>(mappedUser)));
                }
            }
            return BadRequest(new Response<User>(false, user + "is not found"));
        }

        [HttpDelete]
        [Route("[action]{id}")]
        [Authorize(Roles = "Delete")]
        public async Task<ActionResult<Response<bool>>> Delete(Guid id)
        {
            Log.Information($"Deleted {id}");
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
