using System;
using System.Web.Http;
using System.Configuration;
using System.Threading.Tasks;
using MassTransit;

using PoemMessage;

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

        private void HandlePoem(string id, string poem)
        {
            _busControl.Publish<VowelsCalculated>(new
            {
                CorrId = id,
                Text = poem,
                VowelCounts = VowelCalculator.GetVowelCountPerLine(poem)
            });
        }
    }
}
