using ChatRoom.Dtos;
using ChatRoom.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChatRoom.Services.Interfaces
{
    public interface ICrudService<TDto, TID>
        where TDto : Dto<TID>
        where TID : struct
    {
        Task<TDto> GetAsync(TID id);

        Task<TDto> CreateAsync(TDto dto, ApplicationUser user, Action<string, string> AddErrorMessage);

        Task<TDto> UpdateAsync(TDto dto, ApplicationUser user, Action<string, string> AddErrorMessage);

        Task<IEnumerable<TDto>> GetAllAsync(ApplicationUser user);

        Task<TDto> DeleteAsync(TID id, ApplicationUser user, Action<string, string> AddErrorMessage);
    }
}
