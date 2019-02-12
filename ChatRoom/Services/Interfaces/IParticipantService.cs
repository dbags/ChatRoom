using ChatRoom.Dtos;
using ChatRoom.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChatRoom.Services.Interfaces
{
    public interface IParticipantService
    {
        Task<Participant> GetAsync(int id);

        Task<Participant> CreateAsync(Participant dto, ApplicationUser user, Action<string, string> AddErrorMessage);

        Task<IEnumerable<Participant>> CreateRangeAsync(ParticipantRange range, ApplicationUser user, Action<string, string> AddErrorMessage);

        Task<Participant> DeleteAsync(int id, ApplicationUser user, Action<string, string> AddErrorMessage);

        Task<Participant> LeaveChatroomAsync(int chatroomID, ApplicationUser user, Action<string, string> AddErrorMessage);

        /// <summary>
        /// All Participants of the given Chatroom
        /// </summary>
        /// <param name="chatroomID">Chatroom ID</param>
        /// <param name="user">User who requested the list of Participants</param>
        /// <returns>List of Chatroom Participants</returns>
        Task<IEnumerable<Participant>> GetChatroomParticipantsAsync(int chatroomID, ApplicationUser user);

        /// <summary>
        /// Ids and Names of users who doesn't participated to Chatroom
        /// </summary>
        /// <param name="chatroomID">Chatroom ID</param>
        /// <returns>List of Non Participants</returns>
        Task<IEnumerable<UserShort>> GetChatroomNonParticipantsAsync(int chatroomID);

        Task<IEnumerable<Chatroom>> GetParticipatedChatroomsAsync(string userID);
    }
}
