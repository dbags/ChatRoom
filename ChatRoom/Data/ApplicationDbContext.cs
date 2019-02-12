using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ChatRoom.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ChatRoom.Models.ChatroomEntity> ChatroomEntity { get; set; }
        public DbSet<ChatRoom.Models.ParticipantEntity> ParticipantEntity { get; set; }
        public DbSet<ChatRoom.Models.MessageEntity> MessageEntity { get; set; }
    }
}