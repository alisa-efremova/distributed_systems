using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Threading;

namespace VowelCalc
{
    public class ValuesController : ApiController 
    {
        public ValuesController()
        {
        }

        // POST api/values/5
        public IHttpActionResult Post(string id, [FromBody]string value)
        {
            Thread.Sleep(1000);
            return StatusCode(System.Net.HttpStatusCode.OK);
        }
    }
}
