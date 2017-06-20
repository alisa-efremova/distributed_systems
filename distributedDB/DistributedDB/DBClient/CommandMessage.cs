using System;
using System.Collections.Generic;

namespace DBClient
{
    class CommandMessage
    {
        public string Command { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }

        public Dictionary<string, string> ToDictionary()
        {
            return new Dictionary<string, string>
            {
                { "command", Command },
                { "key", Key },
                { "value", Value },
            };
        }
    }
}
