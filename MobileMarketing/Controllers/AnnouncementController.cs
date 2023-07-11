using Application.DTO.Permission;
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
    public class AnnouncementController : ApiControllerBase<Announcement>
    {
        private readonly IAnnouncementRepository _repository;
        public AnnouncementController(IAnnouncementRepository repository)
        {
            _repository = repository;
        }

        [HttpPost]
        [Route("[action]")]
        [Authorize(Roles = "Create")]
        public async Task<IActionResult> Create([FromBody] Announcement announcement)
        {
            Log.Information($"{nameof(Create)}");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _repository.CreateAsync(announcement);
            return Ok(announcement);
        }

        [HttpGet]
        [Route("[action]{id}")]
        [Authorize(Roles = "GetById")]
        public async Task<IActionResult> GetById(Guid id)
        {
            Log.Information($"{nameof(GetById)} Id {id}");
            Announcement? announcement = await _repository.GetAsync(x => x.Id == id);
            if (announcement != null)
            {
                return Ok(announcement);
            }
            return NotFound( id + "is not found!");
        }

        [HttpGet]
        [Route("[action]")]
        [Authorize(Roles = "GetAll")]
        public async Task<IActionResult> GetAll()
        {
            Log.Information($"{nameof(GetAll)}");
            IQueryable<Announcement> announcements = await _repository.GetAllAsync(x => true);
            return Ok(announcements);
        }

        [HttpPut]
        [Route("[action]")]
        [Authorize(Roles = "Update")]
        public async Task<IActionResult> Update([FromBody] Announcement announcement)
        {
            Log.Information($"{nameof(Update)} {announcement.Id}");
            if (ModelState.IsValid)
            {
                Announcement? updated = await _repository.UpdateAsync(announcement);
                if (updated != null)
                {
                    return Ok(updated);
                }
            }
            return BadRequest(announcement + "is not found");
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
            return BadRequest( $"failed to delete the {id}");
        }
    }
}

