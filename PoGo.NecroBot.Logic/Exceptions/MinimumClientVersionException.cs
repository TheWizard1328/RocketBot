using System;

namespace PoGo.NecroBot.Logic.Exceptions
{
    public class MinimumClientVersionException : Exception
    {
        public Version CurrentApiVersion;
        public Version MinimumClientVersion;
        public MinimumClientVersionException(Version currentApiVersion, Version minimumClientVersion) : base()
        {
            CurrentApiVersion = currentApiVersion;
            MinimumClientVersion = minimumClientVersion;
        }
    }
}
