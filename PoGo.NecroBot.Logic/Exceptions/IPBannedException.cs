using System;
using System.Runtime.Serialization;

namespace PoGo.NecroBot.Logic.Exceptions
{
    [Serializable]
    public class IPBannedException : Exception
    {
        public IPBannedException()
        {
        }

        public IPBannedException(string message) : base(message)
        {
        }

        public IPBannedException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected IPBannedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}