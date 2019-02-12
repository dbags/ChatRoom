using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatRoom.Data;
using ChatRoom.Models;
using ChatRoom.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ChatRoom.Repositories.Implementation
{
    public class ParticipantRepository : Repository<ParticipantEntity, int>, IParticipantRepository
    {
        public ParticipantRepository(ApplicationDbContext context) : base(context)
        {
            _completeQuery = _context.ParticipantEntity
                                    .Include(cu => cu.Chatroom)
                                    .Include(cu => cu.User);
        }

        public async Task<IEnumerable<ParticipantEntity>> GetChatroomParticipantsAsync(int chatroomID)
        {
            return await _context.ParticipantEntity
                             .Include(m => m.User)
                             .Where(m => m.ChatroomID == chatroomID)
                             .OrderBy(m => m.User.UserName)
                             .ToListAsync();
        }

        public async Task<IEnumerable<ChatroomEntity>> GetParticipatedChatroomsAsync(string userID)
        { 
            return await _context.ParticipantEntity
                             .Include(m => m.Chatroom)
                             .Where(m => m.UserID == userID && m.Chatroom.OwnerID != userID)
                             .OrderBy(m => m.Chatroom.Title)
                             .Select(m=>m.Chatroom)
                             .ToListAsync();
        }

        public async Task<IEnumerable<IdentityUser>> GetChatroomNonParticipantsAsync(int chatroomID)
        {
            return await (  from u in _context.Users
                            join p in _context.ParticipantEntity on u.Id equals p.UserID into ps
                            from p in ps.DefaultIfEmpty()
                            where !_context.ParticipantEntity
                                .Any(p => p.UserID == u.Id && p.ChatroomID == chatroomID)
                            select u).GroupBy(g=>g.Id).Select(u=>u.FirstOrDefault()).ToListAsync();
        }

    }
}
