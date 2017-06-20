using System;
using System.Web.Http;
using System.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Http;
using System.Net;

namespace Temp
{
    public class DBController : ApiController 
    {
        public HttpResponseMessage Get(string id)
        {
            Console.WriteLine("GET");
            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}
