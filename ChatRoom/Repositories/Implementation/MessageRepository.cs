using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatRoom.Data;
using ChatRoom.Dtos;
using ChatRoom.Models;
using ChatRoom.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ChatRoom.Repositories.Implementation
{
    public class MessageRepository : Repository<MessageEntity, int>, IMessageRepository
    {
        public MessageRepository(ApplicationDbContext context) : base(context)
        {
            _completeQuery = _context.MessageEntity
                                    .Include(m => m.Chatroom)
                                    .Include(m => m.Poster);
        }

        public async Task<IEnumerable<MessageEntity>> GetChatroomMessagesAsync(int chatroomID)
        {
            return await _context.MessageEntity
                                    .Include(m => m.Poster)
                                    .Where(m => m.ChatroomID == chatroomID)
                                    .OrderByDescending(m => m.PostDate)
                                    .ToListAsync();
        }
    }
}
