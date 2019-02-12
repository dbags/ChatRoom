using ChatRoom.Dtos;
using ChatRoom.Mapping.Interfaces;
using ChatRoom.Models;

namespace ChatRoom.Mapping.Implementation
{
    public class ParticipantMappingService : MappingService<Participant, ParticipantEntity, int>, IParticipantMappingService
    {
        public override ParticipantEntity DtoToEntity(Participant dto)
        {
            if (dto == null)
            {
                return null;
            }
            ParticipantEntity entity = new ParticipantEntity
            {
                ID = dto.ID,
                ChatroomID = dto.ChatroomID,
                UserID = dto.UserID,
            };
            return entity;
        }

        public override Participant EntityToDto(ParticipantEntity entity)
        {
            if (entity == null)
            {
                return null;
            }
            Participant dto = new Participant
            {
                ID = entity.ID,
                ChatroomID = entity.ChatroomID,
                ChatroomTitle = entity.Chatroom?.Title,
                UserID = entity.UserID,
                UserName = entity.User?.UserName,
            };
            return dto;
        }
    }
}
