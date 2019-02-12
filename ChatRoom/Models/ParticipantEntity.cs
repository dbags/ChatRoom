namespace ChatRoom.Models
{
    public class ParticipantEntity : Entity<int>
    {
        public int ChatroomID { get; set; }
        public ChatroomEntity Chatroom { get; set; }

        public string UserID { get; set; }
        public ApplicationUser User { get; set; }
    }
}
