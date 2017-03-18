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

        public IHttpActionResult Post(string id, [FromBody]string poem)
        {
            Task.Run(() => HandlePoem(id, poem));
            return StatusCode(System.Net.HttpStatusCode.OK);
        }

        private async void HandlePoem(string id, string poem)
        {
            await _busControl.Publish<PoemFilteringStarted>(new {
                CorrId = id,
                Poem = poem
            });
            
            Uri sendEndpointUri = new Uri(string.Concat(ConfigurationManager.AppSettings["RabbitMQHost"], ConfigurationManager.AppSettings["ConsonantCalcQueueName"]));
            var endpoint = await _busControl.GetSendEndpoint(sendEndpointUri);
            await endpoint.Send<CalculateConsonants>(new
            {
                CorrId = id,
                Text = poem,
                VowelCounts = VowelCalculator.GetVowelCountPerLine(poem)
            });
        }
    }
}
