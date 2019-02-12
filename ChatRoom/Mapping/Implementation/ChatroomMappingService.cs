using ChatRoom.Dtos;
using ChatRoom.Mapping.Interfaces;
using ChatRoom.Models;

namespace ChatRoom.Mapping.Implementation
{
    public class ChatroomMappingService : MappingService<Chatroom, ChatroomEntity, int>,  IChatroomMappingService
    {
        public override ChatroomEntity DtoToEntity(Chatroom dto)
        {
            if (dto == null)
            {
                return null;
            }
            ChatroomEntity entity = new ChatroomEntity
            {
                ID = dto.ID,
                OwnerID = dto.OwnerID,
                Title = dto.Title,
            };
            return entity;
        }

        public override Chatroom EntityToDto(ChatroomEntity entity)
        {
            if (entity == null)
            {
                return null;
            }
            Chatroom dto = new Chatroom
            {
                ID = entity.ID,
                OwnerID = entity.OwnerID,
                OwnerName = entity.Owner?.UserName,
                Title = entity.Title,
            };
            return dto;
        }
    }
}
