using ChatRoom.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChatRoom.Services.Interfaces
{
    public interface IMessageService
    {
        Task<Message> GetAsync(int id);

        Task<Message> CreateAsync(Message dto, Action<string, string> AddErrorMessage);

        Task<Message> DeleteAsync(int id, Action<string, string> AddErrorMessage);

        Task<IEnumerable<Message>> GetChatroomMessagesAsync(int chatroomID);
    }
}
