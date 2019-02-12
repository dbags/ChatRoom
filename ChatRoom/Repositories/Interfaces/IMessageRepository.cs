using ChatRoom.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChatRoom.Repositories.Interfaces
{
    public interface IMessageRepository : IRepository<MessageEntity, int>
    {
        /// <summary>
        /// All messages of the given Chatroom
        /// </summary>
        /// <param name="chatroomID">Chatroom ID</param>
        /// <returns>All messages of the</returns>
        Task<IEnumerable<MessageEntity>> GetChatroomMessagesAsync(int chatroomID);
    }
}
