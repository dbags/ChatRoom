namespace ChatRoom.Models
{
    public abstract class Entity<TID>
        where TID : struct
    {
        public TID ID { get; set; }
    }
}
