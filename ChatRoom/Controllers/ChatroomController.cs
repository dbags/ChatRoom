using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ChatRoom.Dtos;
using ChatRoom.Services.Interfaces;
using ChatRoom.Attributes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ChatRoom.Exceptions;
using Microsoft.AspNetCore.Identity;
using ChatRoom.Models;
using System.Linq;

namespace ChatRoom.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class ChatroomController : ControllerBase
    {
        protected readonly IChatroomService _chatroomService;
        protected readonly IParticipantService _participantService;
        protected readonly UserManager<ApplicationUser> _userManager;

        public ChatroomController(IChatroomService chatroomService
            , IParticipantService participantService
            , UserManager<ApplicationUser> userManager)
        {
            _chatroomService = chatroomService ?? throw new ArgumentNullException(nameof(chatroomService));
            _participantService = participantService ?? throw new ArgumentNullException(nameof(participantService));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IEnumerable<Chatroom>> Index()
        {
            ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
            IEnumerable<Chatroom> chatrooms = await _chatroomService.GetAllAsync(user);
            chatrooms = Enumerable.Concat(chatrooms, await _participantService.GetParticipatedChatroomsAsync(user.Id));
            return chatrooms;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<Chatroom> Get(int id)
        {
            // 200(OK) if found an id
            // 204(No Content) if there is no such id
            return await _chatroomService.GetAsync(id);
        }

        [HttpPost]
        [ValidateModelFilter]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Create([FromBody]Chatroom dto)
        {
            ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
            Chatroom created = await _chatroomService.CreateAsync(dto, user, ModelState.AddModelError);
            Participant participant = await _participantService.CreateAsync(
                        new Participant { ChatroomID = created.ID, UserID = user.Id, }, user, ModelState.AddModelError);

            if (created == null || participant == null)
            {
                return BadRequest(ModelState);
            }

            return CreatedAtAction(
                 nameof(Get),
                 new { id = created.ID },
                 created
             );
        }

        [HttpPut]
        [ValidateModelFilter]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public virtual async Task<IActionResult> Update([FromBody]Chatroom dto)
        {
            try
            {
                ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
                Chatroom updatedDto = await _chatroomService.UpdateAsync(dto, user, ModelState.AddModelError);
                if (updatedDto == null)
                {
                    return BadRequest(ModelState);
                }

                return Ok(updatedDto);
            }
            catch (EntityNotFoundException<Chatroom, int>)
            {
                return NotFound();
            }
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
                Chatroom deletedDto = await _chatroomService.DeleteAsync(id, user, ModelState.AddModelError);
                if (deletedDto == null)
                {
                    return BadRequest(ModelState);
                }
                return Ok(deletedDto);
            }
            catch (EntityNotFoundException<Chatroom, int>)
            {
                return NotFound();
            }
        }
    }
}