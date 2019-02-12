using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatRoom.Attributes;
using ChatRoom.Dtos;
using ChatRoom.Exceptions;
using ChatRoom.Models;
using ChatRoom.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ChatRoom.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ParticipantController : ControllerBase
    {
        protected readonly IParticipantService _participantService;
        protected readonly UserManager<ApplicationUser> _userManager;

        public ParticipantController(IParticipantService participantService
            , UserManager<ApplicationUser> userManager)
        {
            _participantService = participantService ?? throw new ArgumentNullException(nameof(participantService));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        [HttpGet]
        [Route("GetChatroomParticipants")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IEnumerable<Participant>> GetChatroomParticipants(int chatroomID)
        {
            // 200(OK) if found an id
            // 204(No Content) if there is no such id
            ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
            return await _participantService.GetChatroomParticipantsAsync(chatroomID, user);
        }

        [HttpGet]
        [Route("GetChatroomNonParticipants")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IEnumerable<UserShort>> GetChatroomNonParticipants(int chatroomID)
        {
            // 200(OK) if found an id
            // 204(No Content) if there is no such id
            return await _participantService.GetChatroomNonParticipantsAsync(chatroomID);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<Participant> Get(int id)
        {
            // 200(OK) if found an id
            // 204(No Content) if there is no such id
            return await _participantService.GetAsync(id);
        }

        [HttpPost]
        [ValidateModelFilter]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Create([FromBody]Participant dto)
        {
            ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
            Participant created = await _participantService.CreateAsync(dto, user, ModelState.AddModelError);
            if (created == null)
            {
                return BadRequest(ModelState);
            }

            return CreatedAtAction(
                 nameof(Get),
                 new { id = dto.ID },
                 dto
             );
        }

        [HttpPost]
        [Route("CreateRange")]
        [ValidateModelFilter]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IEnumerable<Participant>> CreateRange([FromBody] ParticipantRange range)
        {
            ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
            IEnumerable<Participant> created = await _participantService.CreateRangeAsync(range, user, ModelState.AddModelError);
            if (created == null)
            {
                return null;
            }

            return created;
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public virtual async Task<IActionResult> Delete(int id)
        {
            try
            {
                ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
                Participant deletedDto = await _participantService.DeleteAsync(id, user, ModelState.AddModelError);
                if (deletedDto == null)
                {
                    return BadRequest(ModelState);
                }
                return Ok(deletedDto);
            }
            catch (EntityNotFoundException<Participant, int>)
            {
                return NotFound();
            }
        }

        [HttpGet]
        [Route("LeaveChatroom")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public virtual async Task<IActionResult> LeaveChatroom(int chatroomID)
        {
            try
            {
                ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
                Participant deletedDto = await _participantService.LeaveChatroomAsync(chatroomID, user, ModelState.AddModelError);
                if (deletedDto == null)
                {
                    return BadRequest(ModelState);
                }
                return Ok(deletedDto);
            }
            catch (EntityNotFoundException<Participant, int>)
            {
                return NotFound();
            }
        }
    }
}