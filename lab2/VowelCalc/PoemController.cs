using System;
using System.Web.Http;
using System.Configuration;
using System.Threading.Tasks;
using MassTransit;

using CommandContract;
using EventContract;

namespace VowelCalc
{
    public class PoemController : ApiController 
    {
        private readonly IBusControl _busControl;

        public PoemController()
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

        ~PoemController()
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
                CorrId = postParams.CorrId,
                Poem = poemLines
            });
            
            Uri sendEndpointUri = new Uri(string.Concat(ConfigurationManager.AppSettings["RabbitMQHost"], ConfigurationManager.AppSettings["ConsonantCalcQueueName"]));
            var endpoint = await _busControl.GetSendEndpoint(sendEndpointUri);
            await endpoint.Send<CalculateConsonants>(new
            {
                UserId = postParams.UserId,
                CorrId = postParams.CorrId,
                Text = poemLines,
                VowelCounts = VowelCalculator.GetVowelCountPerLine(poemLines)
            });
        }
    }
}
