using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Newtonsoft.Json;

namespace GoodPoem
{
    class GoodPoemResponse
    {
        public string Poem { get; set; }
        public bool Error { get; set; }
    }

    public class GoodPoemController : ApiController
    {
        private readonly Storage _storage;

        public GoodPoemController()
        {
            _storage = new Storage();
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
                string poem = _storage.Get(id);
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
    }
}
