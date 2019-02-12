namespace ChatRoom.Dtos
{
    public class Chatroom : Dto<int>
    {
        public string OwnerID { get; set; }
        public string OwnerName { get; set; }
        public string Title { get; set; }
        public bool IsOwner { get; set; } = false;
    }
}
