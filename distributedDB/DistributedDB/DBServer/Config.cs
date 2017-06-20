using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace DBServer
{
    public class Config
    {
        public static string BaseURL()
        {
            return ConfigurationManager.AppSettings["WebAPIAddress"];
        }

        public static int ServerId()
        {
            return Int32.Parse(ConfigurationManager.AppSettings["ServerId"]);
        }

        public static string TCPConnection()
        {
            return ConfigurationManager.AppSettings["TCPAddress"];
        }

        public static string[] AllTCPConnections()
        {
            return ConfigurationManager.AppSettings["AllTCPAddresses"].Split(',');
        }
    }
}
