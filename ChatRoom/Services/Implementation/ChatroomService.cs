using ChatRoom.Data;
using ChatRoom.Dtos;
using ChatRoom.Exceptions;
using ChatRoom.Mapping.Interfaces;
using ChatRoom.Models;
using ChatRoom.Repositories.Interfaces;
using ChatRoom.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatRoom.Services.Implementation
{
    public class ChatroomService : IChatroomService
    {
        protected readonly IChatroomRepository _repository;
        protected readonly IChatroomMappingService _mappingService;
        protected readonly ApplicationDbContext _context;

        public ChatroomService(IChatroomRepository repository
            , IChatroomMappingService mappingService
            , ApplicationDbContext context)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mappingService = mappingService ?? throw new ArgumentNullException(nameof(mappingService));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Chatroom> CreateAsync(Chatroom dto, ApplicationUser user, Action<string, string> AddErrorMessage)
        {
            if( user == null )
            {
                AddErrorMessage?.Invoke("General", "Only Logged-in user can perform this operation");
                return null;
            }

            ChatroomEntity entity = _mappingService.DtoToEntity(dto);
            entity.OwnerID = user.Id;
            ChatroomEntity createdEntity = await _repository.CreateAsync(entity);
            if (createdEntity != null)
            {
                await _context.SaveChangesAsync();
            }

            createdEntity = await _repository.GetCompleteAsync(createdEntity.ID);
            Chatroom created = _mappingService.EntityToDto(createdEntity);
            created.IsOwner = true;
            return created;
        }

        public async Task<Chatroom> DeleteAsync(int id, ApplicationUser user, Action<string, string> AddErrorMessage)
        {
            ChatroomEntity deletedEntity = await _repository.SingleOrDefaultAsync(p => p.ID == id);
            if (deletedEntity == null)
            {
                // Use Dto for catching by Controller
                throw new EntityNotFoundException<Chatroom, int>(id);
            }

            if ( !CheckOwner(deletedEntity, user, AddErrorMessage))
            {
                return null;
            }

            if (_repository.Remove(deletedEntity) != null)
            {
                await _context.SaveChangesAsync();
            }
            return _mappingService.EntityToDto(deletedEntity);
        }

        public async Task<IEnumerable<Chatroom>> GetAllAsync(ApplicationUser user)
        {
            if (user == null)
            {
                return null;
            }
            IEnumerable<Chatroom> chatrooms = _mappingService.EntitiesToDtos(await _repository.GetOwnedChatroomsAsync(user.Id));
            foreach(Chatroom cr in chatrooms)
            {
                cr.IsOwner = true;
            }
            return chatrooms;
        }

        public async Task<Chatroom> GetAsync(int id)
        {
            ChatroomEntity entity = await _repository.GetCompleteAsync(id);
            return _mappingService.EntityToDto(entity);
        }

        public async Task<Chatroom> UpdateAsync(Chatroom dto, ApplicationUser user, Action<string, string> AddErrorMessage)
        {
            ChatroomEntity updatedEntity = await _repository.SingleOrDefaultAsync(ab => ab.ID == dto.ID);
            if (updatedEntity == null)
            {
                // Use Dto for catching by Controller
                throw new EntityNotFoundException<Chatroom, int>(dto.ID);
            }
            _context.Entry(updatedEntity).State = EntityState.Detached;

            if (!CheckOwner(updatedEntity, user, AddErrorMessage))
            {
                return null;
            }

            updatedEntity.Title = dto.Title;
            updatedEntity = _repository.Update(updatedEntity);
            if (updatedEntity != null)
            {
                await _context.SaveChangesAsync();
            }

            updatedEntity = await _repository.GetCompleteAsync(updatedEntity.ID);
            return _mappingService.EntityToDto(updatedEntity);
        }

        private bool CheckOwner(ChatroomEntity entity, ApplicationUser user, Action<string, string> AddErrorMessage)
        {
            if (user == null)
            {
                AddErrorMessage?.Invoke("General", "Only Logged-in user can perform this operation");
                return false;
            }

            if (user.Id != entity.OwnerID)
            {
                AddErrorMessage?.Invoke("General", "Only Owner of a Chatroom can perform this operation!");
                return false;
            }

            return true;
        }
    }
}
