using ChatRoom.Data;
using ChatRoom.Dtos;
using ChatRoom.Exceptions;
using ChatRoom.Mapping.Interfaces;
using ChatRoom.Models;
using ChatRoom.Repositories.Interfaces;
using ChatRoom.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatRoom.Services.Implementation
{
    public class ParticipantService : IParticipantService
    {
        protected readonly IParticipantRepository _repository;
        protected readonly IParticipantMappingService _mappingService;
        protected readonly IChatroomService _chatroomService;
        protected readonly IChatroomMappingService _chatroomMappingService;
        protected readonly ApplicationDbContext _context;

        public ParticipantService(IParticipantRepository repository
            , IParticipantMappingService mappingService
            , IChatroomService chatroomService
            , IChatroomMappingService chatroomMappingService
            , ApplicationDbContext context)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mappingService = mappingService ?? throw new ArgumentNullException(nameof(mappingService));
            _chatroomService = chatroomService ?? throw new ArgumentNullException(nameof(chatroomService));
            _chatroomMappingService = chatroomMappingService ?? throw new ArgumentNullException(nameof(chatroomMappingService));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Participant> CreateAsync(Participant dto, ApplicationUser user, Action<string, string> AddErrorMessage)
        {
            if (user == null)
            {
                AddErrorMessage?.Invoke("General", "Only Logged-in user can perform this operation");
                return null;
            }

            ParticipantEntity entity = _mappingService.DtoToEntity(dto);
            Chatroom chatroom = await _chatroomService.GetAsync(entity.ChatroomID);
            if (user.Id != chatroom.OwnerID)
            {
                AddErrorMessage?.Invoke("General", "Only Owner of a Chatroom can add participants!");
                return null;
            }

            ParticipantEntity createdEntity = await _repository.CreateAsync(entity);
            if (createdEntity != null)
            {
                await _context.SaveChangesAsync();
            }

            createdEntity = await _repository.GetCompleteAsync(createdEntity.ID);
            Participant created = _mappingService.EntityToDto(createdEntity);
            created.Deletable = true;
            return created;
        }

        public async Task<IEnumerable<Participant>> CreateRangeAsync(ParticipantRange range, ApplicationUser user, Action<string, string> AddErrorMessage)
        {
            if (range.Users == null || range.Users.Count == 0)
            {
                return null;
            }

            if (user == null)
            {
                AddErrorMessage?.Invoke("General", "Only Logged-in user can perform this operation");
                return null;
            }

            Chatroom chatroom = await _chatroomService.GetAsync(range.ChatroomID);
            if (user.Id != chatroom.OwnerID)
            {
                AddErrorMessage?.Invoke("General", "Only Owner of a Chatroom can add participants!");
                return null;
            }

            List<ParticipantEntity> participants = new List<ParticipantEntity>();
            foreach (UserShort u in range.Users)
            {
                ParticipantEntity temp = new ParticipantEntity
                {
                    ChatroomID = range.ChatroomID,
                    UserID = u.UserID,
                    User = new ApplicationUser { Id = u.UserID, UserName = u.UserName },
                };
                _context.Entry(temp.User).State = EntityState.Unchanged;
                participants.Add(temp);
            }

            IEnumerable<ParticipantEntity> created = await _repository.CreateRangeAsync(participants);
            if (created != null && created.Any())
            {
                await _context.SaveChangesAsync();
            }

            IEnumerable<Participant> result = _mappingService.EntitiesToDtos(created);
            foreach (Participant p in result)
            {
                p.Deletable = (p.UserID != user.Id);
            }

            return result;
        }

        public async Task<Participant> DeleteAsync(int id, ApplicationUser user, Action<string, string> AddErrorMessage)
        {
            ParticipantEntity deletedEntity = await _repository.SingleOrDefaultAsync(p => p.ID == id);
            if (deletedEntity == null)
            {
                // Use Dto for catching by Controller
                throw new EntityNotFoundException<Participant, int>(id);
            }

            Chatroom chatroom = await _chatroomService.GetAsync(deletedEntity.ChatroomID);
            if (user == null)
            {
                AddErrorMessage?.Invoke("General", "Only Logged-in user can perform this operation");
                return null;
            }

            // Participant can be deleted by the Owner of a Chatroom, or itself
            if (user.Id != chatroom.OwnerID && user.Id != deletedEntity.UserID)
            {
                AddErrorMessage?.Invoke("General", "Only Owner of a Chatroom or Participant itself can perform this operation!");
                return null;
            }

            if (_repository.Remove(deletedEntity) != null)
            {
                await _context.SaveChangesAsync();
            }
            return _mappingService.EntityToDto(deletedEntity);
        }

        public async Task<Participant> LeaveChatroomAsync(int chatroomID, ApplicationUser user, Action<string, string> AddErrorMessage)
        {
            ParticipantEntity participantEntity =
                await _repository.SingleOrDefaultAsync(p => p.ChatroomID == chatroomID && p.UserID == user.Id);
            if (participantEntity == null)
            {
                return null;
            }

            ParticipantEntity deletedEntity = _repository.Remove(participantEntity);
            if(deletedEntity != null)
            {
                await _context.SaveChangesAsync();
            }
            return _mappingService.EntityToDto(deletedEntity);
        }

        public async Task<Participant> GetAsync(int id)
        {
            ParticipantEntity entity = await _repository.GetCompleteAsync(id);
            return _mappingService.EntityToDto(entity);
        }

        public async Task<IEnumerable<Participant>> GetChatroomParticipantsAsync(int chatroomID, ApplicationUser user)
        {
            IEnumerable<Participant> participants =
                _mappingService.EntitiesToDtos(await _repository.GetChatroomParticipantsAsync(chatroomID));

            Chatroom chatroom = await _chatroomService.GetAsync(chatroomID);
            if (chatroom.OwnerID == user.Id)
            {
                foreach (Participant p in participants)
                {
                    p.Deletable = (p.UserID != user.Id);
                }
            }

            return participants;
        }

        public async Task<IEnumerable<UserShort>> GetChatroomNonParticipantsAsync(int chatroomID)
        {
            return (await _repository.GetChatroomNonParticipantsAsync(chatroomID))
                    .Select(u => new UserShort { UserID = u.Id, UserName = u.UserName });
        }

        public async Task<IEnumerable<Chatroom>> GetParticipatedChatroomsAsync(string userID)
        {
            return _chatroomMappingService.EntitiesToDtos(await _repository.GetParticipatedChatroomsAsync(userID));
        }

    }
}
