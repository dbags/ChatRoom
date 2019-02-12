using ChatRoom.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChatRoom.Repositories.Interfaces
{
    public interface IChatroomRepository : IRepository<ChatroomEntity, int>
    {
        Task<IEnumerable<ChatroomEntity>> GetOwnedChatroomsAsync(string userID);
    }
}
