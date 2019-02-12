namespace ChatRoom.Dtos
{
    public abstract class Dto<TID>
        where TID : struct
    {
        public TID ID { get; set; }
    }
}
