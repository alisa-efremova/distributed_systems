using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin.Hosting;

namespace Temp
{
    class Program
    {
        static void Main(string[] args)
        {
            using (WebApp.Start<Startup>(url: "http://localhost:7781/"))
            {
                Console.WriteLine("Press Enter to exit.");
                Console.ReadLine();
            }
        }
    }
}
