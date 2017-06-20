using System;
using System.Web.Http;
using System.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using NNanomsg.Protocols;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net;

namespace DBServer
{
    public class DBController : ApiController
    {
        private static readonly PublishSocket _publishSocket = new PublishSocket();
        private static readonly DBManager _db = new DBManager();

        public DBController()
        {
            _publishSocket.Bind(Config.TCPConnection());
            Thread.Sleep(500); // time to connect
        }

        ~DBController()
        {
            _publishSocket.Dispose();
        }

        public class RequestPostParams
        {
            public string Command { get; set; }
            public string Key { get; set; }
            public string Value { get; set; }
        }

        public class ResponseContent
        {
            public bool KeyExists { get; set; }
            public string Value { get; set; }
        }

        public HttpResponseMessage Post([FromBody]RequestPostParams postParams)
        {
            return HandlePostParams(postParams);
        }

        private HttpResponseMessage HandlePostParams(RequestPostParams postParams)
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            if (postParams.Command == "get")
            {
                string value = _db.Get(postParams.Key);
                var responseContent = new ResponseContent
                {
                    KeyExists = (value != null),
                    Value = (value != null) ? value : "",
                };
                response.Content = new StringContent(JsonConvert.SerializeObject(responseContent));
            }
            else
            {
                if (!ProceedWrite(postParams.Command, postParams.Key, postParams.Value))
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                }
            }
            return response;
        }

        private bool ProceedWrite(string command, string key, string value)
        {
            var commandMessage = new DBUpdateMessage
            {
                Command = command,
                Key = key,
                Value = value,
                ServerId = Config.ServerId(),
                Version = _db.VersionForKey(key) + 1
            };

            if (!commandMessage.isValid())
            {
                return false;
            }

            try
            {
                _publishSocket.Send(commandMessage.Encode());
                return true;
            }
            catch
            {
                Console.WriteLine("Exception while trying to publish db updates");
                return false;
            }
        }
    }
}
