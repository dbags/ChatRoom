using ChatRoom.Dtos;
using ChatRoom.Mapping.Interfaces;
using ChatRoom.Models;

namespace ChatRoom.Mapping.Implementation
{
    public class MessageMappingService : MappingService<Message, MessageEntity, int>, IMessageMappingService
    {
        public override MessageEntity DtoToEntity(Message dto)
        {
            if (dto == null)
            {
                return null;
            }
            MessageEntity entity = new MessageEntity
            {
                ID = dto.ID,
                ChatroomID = dto.ChatroomID,
                Content = dto.Content,
                PostDate = dto.PostDate,
                PosterID = dto.PosterID,
            };
            return entity;
        }

        public override Message EntityToDto(MessageEntity entity)
        {
            if (entity == null)
            {
                return null;
            }
            Message dto = new Message
            {
                ID = entity.ID,
                ChatroomID = entity.ChatroomID,
                ChatroomTitle = entity.Chatroom?.Title,
                Content = entity.Content,
                PosterID = entity.PosterID,
                PosterName = entity.Poster?.UserName,
                PostDate = entity.PostDate,
            };
            return dto;
        }
    }
}
