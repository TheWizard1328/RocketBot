#region using directives

using System;

#endregion

namespace PoGo.NecroBot.Logic.Exceptions
{
    public class GoogleException : Exception
    {
        public GoogleException(string message) : base(message)
        {
        }
    }
}