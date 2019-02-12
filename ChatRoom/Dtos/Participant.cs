using System.Collections.Generic;

namespace ChatRoom.Dtos
{
    public class Participant : Dto<int>
    {
        public int ChatroomID { get; set; }
        public string ChatroomTitle { get; set; }

        public string UserID { get; set; }
        public string UserName { get; set; }

        public bool Deletable { get; set; } = false;
    }

    public class ParticipantRange
    {
        public int ChatroomID { get; set; }
        public List<UserShort> Users { get; set; }
    }

    public class UserShort
    {
        public string UserID { get; set; }
        public string UserName { get; set; }
    }
}
