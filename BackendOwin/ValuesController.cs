using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Threading;

namespace BackendOwin
{
    public class ValuesController : ApiController 
    {
        private readonly RedisStorage _storage;

        public ValuesController()
        {
            _storage = new RedisStorage();
        }

        // POST api/values/5
        public IHttpActionResult Post(string id, [FromBody]string value)
        {
            Thread.Sleep(1000);

            _storage.Save(id, value);

            return StatusCode(System.Net.HttpStatusCode.OK);
        }
    }
}
