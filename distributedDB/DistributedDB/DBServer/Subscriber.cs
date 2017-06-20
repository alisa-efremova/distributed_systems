using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using NNanomsg.Protocols;

namespace DBServer
{
    public class Subscriber
    {
        private SubscribeSocket _socket;
        private DBManager _db;

        public Subscriber(string[] connections, string subscription = "")
        {
            _socket = new SubscribeSocket();

            foreach (string connection in connections)
            {
                _socket.Connect(connection);
            }
            _socket.Subscribe(subscription);
            Thread.Sleep(100); // time to connect

            _db = new DBManager();
        }

        ~Subscriber()
        {
            _socket.Dispose();
        }

        public void Execute()
        {
            byte[] buffer;

            while (true)
            {
                Console.WriteLine("Listening...");

                try
                {
                    buffer = _socket.Receive();

                    var message = DBUpdateMessage.Decode(buffer);
                    Console.WriteLine("\t[" + message.ServerId + "] Received command '" + message.Command + "' for key: " + message.Key);
                    ProceedWriteCommand(message);
                    Console.WriteLine("\tOK");
                }
                catch
                {
                    Console.WriteLine("Catch exception in subscriber");
                }
            }
        }

        private void ProceedWriteCommand(DBUpdateMessage message)
        {
            var dbValue = new DBValue
            {
                Value = message.Value,
                Version = message.Version,
                ServerId = message.ServerId
            };

            if (message.Command == "set")
            {
                _db.Set(message.Key, dbValue);
            }
            else if (message.Command == "remove")
            {
                _db.Remove(message.Key, dbValue);
            }
            else
            {
                Console.WriteLine("Unknown command");
            }
        }
    }
}
