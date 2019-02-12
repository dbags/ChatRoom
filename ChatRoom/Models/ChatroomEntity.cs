using System.Collections.Generic;

namespace ChatRoom.Models
{
    public class ChatroomEntity : Entity<int>
    {
        public string Title { get; set; }

        public string OwnerID { get; set; }
        public ApplicationUser Owner { get; set; }

        public List<ParticipantEntity> Participants { get; set; }
    }
}
