#region using directives

using System;

#endregion

namespace PoGo.NecroBot.Logic.Exceptions
{
    
    public class SessionInvalidatedException : Exception
    {
        public SessionInvalidatedException()
        {
        }

        public SessionInvalidatedException(string message) : base(message)
        {
        }
    }
}