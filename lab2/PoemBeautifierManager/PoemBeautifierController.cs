using System;
using System.Web.Http;
using System.Configuration;
using System.Threading.Tasks;
using MassTransit;

using PoemFilterContract;
using PoemBeautifierContract;

namespace PoemBeautifierManager
{
    public class PoemBeautifierController : ApiController 
    {
        private readonly IBusControl _busControl;

        public PoemBeautifierController()
        {
            _busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                var host = cfg.Host(new Uri(ConfigurationManager.AppSettings["RabbitMQHost"]), h =>
                {
                    h.Username(ConfigurationManager.AppSettings["RabbitMQUsername"]);
                    h.Password(ConfigurationManager.AppSettings["RabbitMQPassword"]);
                });
            });
            _busControl.Start();
        }

        ~PoemBeautifierController()
        {
            _busControl.Stop();
        }

        public class ResourceQuery
        {
            public string CorrId { get; set; }
            public string UserId { get; set; }
            public string Poem { get; set; }
        }

        public IHttpActionResult Post([FromBody]ResourceQuery postParams)
        {
            Task.Run(() => HandlePostParams(postParams));
            return StatusCode(System.Net.HttpStatusCode.OK);
        }

        private async void HandlePostParams(ResourceQuery postParams)
        {
            var poemLines = postParams.Poem.Split('\n');
            await _busControl.Publish<PoemFilteringStarted>(new {
                UserId = postParams.UserId,
                CorrId = postParams.CorrId,
                Poem = poemLines
            });
        }
    }
}
