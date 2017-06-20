using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Configuration;
using System.Net.Http;
using System.Net;
using Newtonsoft.Json;

namespace DBClient
{
    public class ResponseContent
    {
        public bool KeyExists { get; set; }
        public string Value { get; set; }
    }

    class Program
    {
        private static Dictionary<string, int> _commandsArgCountMap = new Dictionary<string, int> { 
            { "set", 2 },
            { "get", 1 },
            { "remove", 1 },
        };

        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Client app to manage key-value database.");
                PrintHelp();
            }
            else
            {
                var command = ParseCommandFromArgs(args);
                if (command != null)
                {
                    var task = SendData(command.ToDictionary());
                    task.Wait();
                    var response = task.Result;
                    if (command.Command == "get" && response.IsSuccessStatusCode && response.Content != null)
                    {
                        ProcessResponseContent(response);
                    }
                    else
                    {
                        Console.WriteLine("Result: " + response.StatusCode);
                    }
                }
                else
                {
                    PrintHelp();
                }
            }
        }

        private static void ProcessResponseContent(HttpResponseMessage response)
        {
            var readTask = response.Content.ReadAsStringAsync();
            readTask.Wait();
            var responseContent = JsonConvert.DeserializeObject<ResponseContent>(readTask.Result);
            if (responseContent == null)
            {
                Console.WriteLine("Invalid response content");
            }
            else
            {
                if (responseContent.KeyExists)
                {
                    Console.WriteLine("Value: " + responseContent.Value);
                }
                else
                {
                    Console.WriteLine("Key doesn't exists.");
                }
            }
        }

        private static CommandMessage ParseCommandFromArgs(string[] args)
        {
            string command = args[0];
            if (_commandsArgCountMap.ContainsKey(command) && _commandsArgCountMap[command] == args.Length - 1)
            {
                return new CommandMessage
                {
                    Command = command,
                    Key = args[1],
                    Value = (_commandsArgCountMap[command] == 2) ? args[2] : null
                };
            }
            else
            {
                Console.WriteLine("Invalid command.");
                return null;
            }
        }

        static void PrintHelp()
        {
            Console.WriteLine("Commands help:");
            Console.WriteLine("\tset <key> <value>");
            Console.WriteLine("\tget <key>");
            Console.WriteLine("\tremove <key>");
        }

        static async Task<HttpResponseMessage> SendData(Dictionary<string, string> data)
        {
            string url = ConfigurationManager.AppSettings["ServerAddress"] + "api/DB";
            var content = new FormUrlEncodedContent(data);

            using (HttpClient httpClient = new HttpClient())
            {
                HttpResponseMessage responseMessage = null;
                try
                {
                    responseMessage = await httpClient.PostAsync(url, content);
                }
                catch
                {
                    if (responseMessage == null)
                    {
                        responseMessage = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                    }
                }
                return responseMessage;
            }
        }
    }
}
