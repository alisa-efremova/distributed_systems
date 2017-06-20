using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin.Hosting;

namespace DBServer
{
    class Program
    {
        static void Main(string[] args)
        {
            var subscriber = new Subscriber(Config.AllTCPConnections());
            Task.Run(() => subscriber.Execute());

            using (WebApp.Start<Startup>(url: Config.BaseURL()))
            {
                Console.WriteLine("Press Enter to exit.");
                Console.ReadLine();
            }
        }
    }
}
