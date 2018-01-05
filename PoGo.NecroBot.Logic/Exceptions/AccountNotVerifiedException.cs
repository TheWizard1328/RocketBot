#region using directives

using System;

#endregion

namespace PoGo.NecroBot.Logic.Exceptions
{
    public class AccountNotVerifiedException : Exception
    {
        public AccountNotVerifiedException(string message) : base(message)
        {
        }
    }
}