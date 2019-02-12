using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ChatRoom.Attributes;
using ChatRoom.Dtos;
using ChatRoom.Exceptions;
using ChatRoom.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChatRoom.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MessageController : ControllerBase
    {
        protected readonly IMessageService _messageService;

        public MessageController(IMessageService messageService)
        {
            _messageService = messageService ?? throw new ArgumentNullException(nameof(messageService));
        }

        [HttpGet]
        [Route("GetChatroomMessages")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IEnumerable<Message>> GetChatroomMessages(int chatroomID)
        {
            // 200(OK) if found an id
            // 204(No Content) if there is no such id
            return await _messageService.GetChatroomMessagesAsync(chatroomID);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<Message> Get(int id)
        {
            // 200(OK) if found an id
            // 204(No Content) if there is no such id
            return await _messageService.GetAsync(id);
        }

        [HttpPost]
        [ValidateModelFilter]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Create([FromBody]Message dto)
        {
            Message created = await _messageService.CreateAsync(dto, ModelState.AddModelError);
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

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public virtual async Task<IActionResult> Delete(int id)
        {
            try
            {
                Message deletedDto = await _messageService.DeleteAsync(id, ModelState.AddModelError);
                if (deletedDto == null)
                {
                    return BadRequest(ModelState);
                }
                return Ok(deletedDto);
            }
            catch (EntityNotFoundException<Message, int>)
            {
                return NotFound();
            }
        }
    }
}