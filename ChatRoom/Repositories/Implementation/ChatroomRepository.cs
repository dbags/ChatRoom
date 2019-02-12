using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatRoom.Data;
using ChatRoom.Models;
using ChatRoom.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ChatRoom.Repositories.Implementation
{
    public class ChatroomRepository : Repository<ChatroomEntity, int>, IChatroomRepository
    {
        public ChatroomRepository(ApplicationDbContext context) : base(context)
        {
            _completeQuery = _context.ChatroomEntity
                                    .Include(c => c.Owner)
                                    .Include(c => c.Participants)
                                        .ThenInclude(p => p.User);
        }

        public async Task<IEnumerable<ChatroomEntity>> GetOwnedChatroomsAsync(string userID)
        {
            return await _context.ChatroomEntity.Where(c => c.OwnerID == userID).ToListAsync();
        }
    }
}
