using ChatRoom.Dtos;
using ChatRoom.Mapping.Interfaces;
using ChatRoom.Models;
using System.Collections.Generic;

namespace ChatRoom.Mapping.Implementation
{
    public class MappingService<TDto, TEntity, TID> : IMappingService<TDto, TEntity, TID>
        where TDto : Dto<TID>
        where TEntity : Entity<TID>
        where TID : struct
    {
        public virtual IEnumerable<TEntity> DtosToEntities(IEnumerable<TDto> dtos)
        {
            if (dtos == null)
            {
                return null;
            }

            List<TEntity> entities = new List<TEntity>();
            foreach (TDto d in dtos)
            {
                entities.Add(DtoToEntity(d));
            }
            return entities;
        }

        public virtual TEntity DtoToEntity(TDto dto)
        {
            if (dto == null)
            {
                return null;
            }
            return dto as TEntity;
        }

        public virtual IEnumerable<TDto> EntitiesToDtos(IEnumerable<TEntity> entities)
        {
            if (entities == null)
            {
                return null;
            }

            List<TDto> dtos = new List<TDto>();
            foreach (TEntity e in entities)
            {
                dtos.Add(EntityToDto(e));
            }
            return dtos;
        }

        public virtual TDto EntityToDto(TEntity entity)
        {
            if (entity == null)
            {
                return null;
            }
            return entity as TDto;
        }
    }
}
