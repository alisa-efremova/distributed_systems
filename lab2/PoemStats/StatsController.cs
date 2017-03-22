using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace PoemStats
{
    public class StatsController : ApiController
    {
        private readonly Stats _stats;

        public StatsController()
        {
            _stats = new Stats();
        }

        // GET api/stats/
        public double Get()
        {
            return _stats.GetGoodLinesPercent();
        } 
    }
}

