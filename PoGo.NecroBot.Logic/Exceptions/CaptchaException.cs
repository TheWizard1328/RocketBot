using System;

namespace PoGo.NecroBot.Logic.Exceptions
{
    public class CaptchaException : Exception
    {
        public string Url { get; set; }

        public CaptchaException(string url)
        {
            Url = url;
        }
    }
}
