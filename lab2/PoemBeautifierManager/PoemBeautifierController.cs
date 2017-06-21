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

        public class PostParams
        {
            public string CorrId { get; set; }
            public string UserId { get; set; }
            public string Poem { get; set; }
        }

        public IHttpActionResult Post([FromBody]PostParams postParams)
        {
            Task.Run(() => HandlePostParams(postParams));
            return StatusCode(System.Net.HttpStatusCode.OK);
        }

        private async void HandlePostParams(PostParams postParams)
        {
            var poemLines = postParams.Poem.Split('\n');
            await SendMessage(ConfigurationManager.AppSettings["VowelCalcQueueName"], postParams.UserId, postParams.CorrId, poemLines);
            await SendMessage(ConfigurationManager.AppSettings["PoemStatsQueueName"], postParams.UserId, postParams.CorrId, poemLines);
        }

        private async Task SendMessage(string queue, string userId, string corrId, string[] poemLines)
        {
            Uri sendEndpointUri = new Uri(string.Concat(ConfigurationManager.AppSettings["RabbitMQHost"], queue));
            var endpoint = await _busControl.GetSendEndpoint(sendEndpointUri);
            await endpoint.Send<ProceedOriginalPoem>(new
            {
                UserId = userId,
                CorrId = corrId,
                Poem = poemLines
            });
        }
    }
}
