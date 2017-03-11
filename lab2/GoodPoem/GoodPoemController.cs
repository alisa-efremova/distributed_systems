using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Runtime.Caching;
using Newtonsoft.Json;
using Polly;
using Polly.Retry;

namespace GoodPoem
{
    class GoodPoemResponse
    {
        public string Poem { get; set; }
        public bool Error { get; set; }
    }

    class PoemNotFoundException : SystemException { }

    public class GoodPoemController : ApiController
    {
        const int _retryCount = 5;
        const int _initialRetryTimeoutSec = 2;
        Policy _policy;

        public GoodPoemController()
        {
            _policy = Policy
                .Handle<Exception>()
                .WaitAndRetry(_retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(_initialRetryTimeoutSec, retryAttempt)));
        }

        // GET api/goodpoem/corrId
        public string Get(string id)
        {
            Console.WriteLine("Get string");
            GoodPoemResponse response = new GoodPoemResponse();

            try
            {
                Console.WriteLine("-----------------");
                Console.WriteLine("Try get result");
                string poem = GetPoem(id);
                Console.WriteLine("Success");
                response.Error = false;
                response.Poem = poem;
            }
            catch
            {
                Console.WriteLine("Failed");
                response.Error = true;
                response.Poem = "";
            }
            return JsonConvert.SerializeObject(response);
        } 

        string GetPoem(string corrId)
        {
            return _policy.Execute(() =>
            {
                Console.WriteLine("Request poem");
                var poem = MemoryCache.Default.Get(corrId);
                if (poem == null)
                {
                    Console.WriteLine("Throw exception");
                    throw new PoemNotFoundException();
                }
                return (string)poem;
            });
        }
    }
}
