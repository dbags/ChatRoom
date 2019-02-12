using System;

namespace ChatRoom.Exceptions
{
    public class EntityNotFoundException<TEntity, TID> : Exception
        where TEntity : class   // A reference type
        where TID : struct      // A value type
    {
        public EntityNotFoundException(TID id)
            : base($"The entity of type {nameof(TEntity)} with id={id} was not found.")
        {

        }
    }

}
