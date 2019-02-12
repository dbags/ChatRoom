using ChatRoom.Models;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChatRoom.Repositories.Interfaces
{
    public interface IParticipantRepository : IRepository<ParticipantEntity, int>
    {
        /// <summary>
        /// All Participants of the given Chatroom
        /// </summary>
        /// <param name="chatroomID">Chatroom ID</param>
        /// <returns>List of Chatroom Participants</returns>
        Task<IEnumerable<ParticipantEntity>> GetChatroomParticipantsAsync(int chatroomID);

        Task<IEnumerable<IdentityUser>> GetChatroomNonParticipantsAsync(int chatroomID);

        /// <summary>
        /// All Chatrooms there the given user is participated
        /// </summary>
        /// <param name="userID">User ID</param>
        /// <returns>List of Chatrooms</returns>
        Task<IEnumerable<ChatroomEntity>> GetParticipatedChatroomsAsync(string userID);
    }
}
