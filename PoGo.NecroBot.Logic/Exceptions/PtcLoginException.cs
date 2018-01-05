using System;

namespace PoGo.NecroBot.Logic.Exceptions
{
    public class PtcLoginException  : Exception
    {
        public PtcLoginException(string message) : base(message) { }
    }
}
