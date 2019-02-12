using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace ChatRoom.Hubs
{
    public class ChatHub : Hub
    {
        public async Task DeleteChatroom(int chatroomID)
        {
            await Clients.Others.SendAsync("ChatroomDeleted", chatroomID);
        }

        public async Task AddedToChatroom(int chatroomID, int participantID, string userName)
        {
            await Clients.Others.SendAsync("UserAdded", chatroomID, participantID, userName);
        }

        public async Task LeaveChatroom(int chatroomID, int participantID)
        {
            await Clients.Others.SendAsync("UserLeft", chatroomID, participantID);
        }
    }
}
