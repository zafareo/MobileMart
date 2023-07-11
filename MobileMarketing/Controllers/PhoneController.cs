using Application.DTO.Permission;
using Application.DTO.Phone;
using Application.DTO.User;
using Application.Interfaces;
using Application.Models;
using Domain.Models.Entities;
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
    public class PhoneController : ApiControllerBase<Phone>
    {
        private readonly IPhoneRepository _repository;
        public PhoneController(IPhoneRepository repository)
        {
            _repository = repository;
        }


        [HttpGet]
        [Route("[action]")]
        [AllowAnonymous]
        public async Task<ActionResult<Response<Pagination<Phone>>>> Search(string text, int page = 1, int pageSize = 10)
        {
            var Phones = await _repository.GetAllAsync(x => x.Price.ToString().Contains(text)
                                                       || x.Description.Contains(text)
                                                       || x.Name.Contains(text));

            Pagination<Phone> phones = await Pagination<Phone>.CreateAsync(Phones, page, pageSize);

            Response<Pagination<Phone>> res = new()
            {
                Result = phones
            };
            return Ok(res);
        }


        [HttpPost]
        [Route("[action]")]
        [Authorize(Roles = "Create")]
        public async Task<ActionResult<Response<PhoneGetDTO>>> Create([FromBody] PhoneCreateDTO phone)
        {
            Log.Information($"{nameof(Create)} {phone.Name}");
            Phone? mappedPhone = _mapper.Map<Phone>(phone);
            //var validationRes = _validator.Validate(mappedPhone);
            if (ModelState.IsValid)
            {
                mappedPhone = await _repository.CreateAsync(mappedPhone);
                var res = _mapper.Map<PhoneGetDTO>(mappedPhone);
                return Ok(new Response<PhoneGetDTO>(res));
            }
            return BadRequest(new Response<object>(false, ModelState.ValidationState));
        }

        [HttpGet]
        [Route("[action]{id}")]
        [Authorize(Roles = "GetById")]
        public async Task<ActionResult<Response<PhoneGetDTO>>> GetById(Guid id)
        {
            Log.Information($"{nameof(GetById)} Id {id}");
            Phone? phone = await _repository.GetAsync(x => x.Id == id);
            if (phone != null)
            {
                PhoneGetDTO mappedPer = _mapper.Map<PhoneGetDTO>(phone);
                return Ok(new Response<PhoneGetDTO>(mappedPer));
            }
            return NotFound(new Response<Phone>(false, id + "is not found!"));
        }

        [HttpGet]
        [Route("[action]")]
        //[Authorize(Roles = "GetAllPhones")]
        public async Task<ActionResult<Response<Pagination<Phone>>>> GetAllPhones(int page = 1, int pageSize = 10)
        {
            IQueryable<Phone> Phones = await _repository.GetAllAsync(x => true);

            Pagination<Phone> phones = await Pagination<Phone>.CreateAsync(Phones, page, pageSize);

            Response<Pagination<Phone>> res = new()
            {
                Result = phones
            };
            return Ok(res);
        }

        [HttpGet]
        [Route("[action]")]
        [Authorize(Roles = "GetAll")]
        public async Task<ActionResult<Response<IQueryable<PhoneGetDTO>>>> GetAll()
        {
            Log.Information($"{nameof(GetAll)}");
            IQueryable<Phone> phones = await _repository.GetAllAsync(x => true);
            IQueryable<PhoneGetDTO> mappedPhone = _mapper.Map<IQueryable<PhoneGetDTO>>(phones);
            return Ok(new Response<IQueryable<PhoneGetDTO>>(mappedPhone));
        }

        [HttpPut]
        [Route("[action]")]
        [Authorize(Roles = "Update")]
        public async Task<ActionResult<Response<PhoneGetDTO>>> Update([FromBody] PhoneUpdateDTO phone)
        {
            Log.Information($"{nameof(Update)} {phone.Name}");
            Phone? mappedPhone = _mapper.Map<Phone>(phone);
            var validationResult = _validator.Validate(mappedPhone);
            if (validationResult.IsValid)
            {
                mappedPhone = await _repository.UpdateAsync(mappedPhone);
                if (mappedPhone != null)
                {
                    return Ok(new Response<PhoneGetDTO>(_mapper.Map<PhoneGetDTO>(mappedPhone)));
                }
            }
            return BadRequest(new Response<Phone>(false, phone + "is not found"));
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
