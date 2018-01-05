﻿#region using directives

using System;

#endregion

namespace PoGo.NecroBot.Logic.Exceptions
{
    
    public class LoginFailedException : Exception
    {

        public LoginFailedException()
        {
        }

        public LoginFailedException(string message) : base(message)
        {
        }
    }

    public class TokenRefreshException : Exception
    {

        public TokenRefreshException()
        {
        }

        public TokenRefreshException(string message) : base(message)
        {
        }
    }

}