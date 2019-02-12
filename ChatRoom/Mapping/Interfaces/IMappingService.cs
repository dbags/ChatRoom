using ChatRoom.Dtos;
using ChatRoom.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatRoom.Mapping.Interfaces
{
    public interface IMappingService<TDto, TEntity, TID>
        where TDto : Dto<TID>
        where TEntity : Entity<TID>
        where TID : struct
    {
        TEntity DtoToEntity(TDto dto);
        TDto EntityToDto(TEntity entity);

        IEnumerable<TEntity> DtosToEntities(IEnumerable<TDto> dtos);
        IEnumerable<TDto> EntitiesToDtos(IEnumerable<TEntity> entity);
    }
}
