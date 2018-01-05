using System;

namespace PoGo.NecroBot.Logic.Exceptions
{
    public class APIBadRequestException:Exception
    {
        public APIBadRequestException(string message) : base(message)
        {

        }
    }
}
