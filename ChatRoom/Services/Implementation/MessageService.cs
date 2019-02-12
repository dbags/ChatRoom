using ChatRoom.Data;
using ChatRoom.Dtos;
using ChatRoom.Exceptions;
using ChatRoom.Mapping.Interfaces;
using ChatRoom.Models;
using ChatRoom.Repositories.Interfaces;
using ChatRoom.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatRoom.Services.Implementation
{
    public class MessageService : IMessageService
    {
        protected readonly IMessageRepository _repository;
        protected readonly IMessageMappingService _mappingService;
        protected readonly IChatroomService _chatroomService;
        protected readonly UserManager<ApplicationUser> _userManager;
        protected readonly IHttpContextAccessor _httpContextAccessor;
        protected readonly ApplicationDbContext _context;

        public MessageService(IMessageRepository repository
            , IMessageMappingService mappingService
            , IChatroomService chatroomService
            , UserManager<ApplicationUser> userManager
            , IHttpContextAccessor httpContextAccessor
            , ApplicationDbContext context)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mappingService = mappingService ?? throw new ArgumentNullException(nameof(mappingService));
            _chatroomService = chatroomService ?? throw new ArgumentNullException(nameof(chatroomService));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Message> CreateAsync(Message dto, Action<string, string> AddErrorMessage)
        {
            ApplicationUser user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            if (user == null)
            {
                AddErrorMessage?.Invoke("General", "Only Logged-in user can perform this operation");
                return null;
            }

            MessageEntity entity = _mappingService.DtoToEntity(dto);
            Chatroom chatroom = await _chatroomService.GetAsync(entity.ChatroomID);
            if (user.Id != chatroom.OwnerID)
            {
                AddErrorMessage?.Invoke("General", "Only Owner of a Chatroom can add participants!");
                return null;
            }

            MessageEntity createdEntity = await _repository.CreateAsync(entity);
            if (createdEntity != null)
            {
                await _context.SaveChangesAsync();
            }

            createdEntity = await _repository.GetCompleteAsync(createdEntity.ID);
            return _mappingService.EntityToDto(createdEntity);
        }

        public async Task<Message> DeleteAsync(int id, Action<string, string> AddErrorMessage)
        {
            MessageEntity deletedEntity = await _repository.SingleOrDefaultAsync(p => p.ID == id);
            if (deletedEntity == null)
            {
                // Use Dto for catching by Controller
                throw new EntityNotFoundException<Message, int>(id);
            }

            Chatroom chatroom = await _chatroomService.GetAsync(deletedEntity.ChatroomID);
            ApplicationUser user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            if (user == null)
            {
                AddErrorMessage?.Invoke("General", "Only Logged-in user can perform this operation");
                return null;
            }

            // Message can be deleted by the Owner of a Chatroom, or Poster
            if (user.Id != chatroom.OwnerID && user.Id != deletedEntity.PosterID)
            {
                AddErrorMessage?.Invoke("General", "Only Owner of a Chatroom or Poster itself can perform this operation!");
                return null;
            }

            if (_repository.Remove(deletedEntity) != null)
            {
                await _context.SaveChangesAsync();
            }
            return _mappingService.EntityToDto(deletedEntity);
        }

        public async Task<Message> GetAsync(int id)
        {
            MessageEntity entity = await _repository.GetCompleteAsync(id);
            return _mappingService.EntityToDto(entity);
        }

        public async Task<IEnumerable<Message>> GetChatroomMessagesAsync(int chatroomID)
        {
            return _mappingService.EntitiesToDtos(await _repository.GetChatroomMessagesAsync(chatroomID));
        }

    }
}
