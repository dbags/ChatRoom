using ChatRoom.Dtos;
using ChatRoom.Models;

namespace ChatRoom.Mapping.Interfaces
{
    public interface IMessageMappingService : IMappingService<Message, MessageEntity, int>
    {
    }
}
