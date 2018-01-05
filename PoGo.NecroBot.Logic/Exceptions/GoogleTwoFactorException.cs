#region using directives

using System;

#endregion

namespace PoGo.NecroBot.Logic.Exceptions
{
    public class GoogleTwoFactorException : Exception
    {
        public GoogleTwoFactorException(string message) : base(message)
        {
        }
    }
}