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
        // GET api/stats/
        public double Get()
        {
            return Stats.GetInstance().GetGoodLinesPercent();
        } 
    }
}

