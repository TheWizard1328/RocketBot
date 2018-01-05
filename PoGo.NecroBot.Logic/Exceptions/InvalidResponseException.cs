#region using directives

using System;

#endregion

namespace PoGo.NecroBot.Logic.Exceptions
{
    public class InvalidResponseException : Exception
    {
        public InvalidResponseException()
        {
        }

        public InvalidResponseException(string message)
            : base(message)
        {
        }
    }
}