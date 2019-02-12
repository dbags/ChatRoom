using ChatRoom.Dtos;
using ChatRoom.Models;

namespace ChatRoom.Mapping.Interfaces
{
    public interface IParticipantMappingService : IMappingService<Participant, ParticipantEntity, int>
    {
    }
}
