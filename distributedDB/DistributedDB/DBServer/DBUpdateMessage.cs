using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CS;

namespace DBServer
{
    public class DBUpdateMessage
    {
        private static string[] _commands = new string[] { "set", "remove" };

        public string Command { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public int Version { get; set; }
        public int ServerId { get; set; }

        public bool isValid()
        {
            if (Command == null || !_commands.Contains(Command))
            {
                return false;
            }

            if (Key == null || Version == 0 || ServerId == 0)
            {
                return false;
            }

            if (Command == "set" && Value == null)
            {
                return false;
            }

            return true;
        }

        public byte[] Encode()
        {
            MPackMap dictionary = new MPackMap
            {
                { "command", Command },
                { "key",  Key },
                { "value", (Value == null) ? "" : Value },
                { "serverId", ServerId }, 
                { "version", Version }
            };
            return dictionary.EncodeToBytes();
        }

        public static DBUpdateMessage Decode(byte[] bytes)
        {
            try
            {
                var message = new DBUpdateMessage();

                var decoded = MPack.ParseFromBytes(bytes);
                message.Command = decoded["command"].To<string>();
                message.Key = decoded["key"].To<string>();
                message.Value = (message.Command == "remove") ? null : decoded["value"].To<string>();
                message.Version = decoded["version"].To<int>();
                message.ServerId = decoded["serverId"].To<int>();

                return message;
            }
            catch
            {
                return null;
            }

        }
    }
}
