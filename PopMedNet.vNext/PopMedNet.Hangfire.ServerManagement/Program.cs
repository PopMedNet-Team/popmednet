using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hangfire;
using Hangfire.SqlServer;

namespace PopMedNet.Hangfire.ServerManagement
{
    class Program
    {
        static void Main(string[] args)
        {
            GlobalConfiguration.Configuration.UseSqlServerStorage(ConfigurationManager.ConnectionStrings["DataContext"].ConnectionString);

            var servers = GetServers();            

            int index = -1;
            var keyInfo = Console.ReadLine();
            while (!string.IsNullOrEmpty(keyInfo))
            {
                if (int.TryParse(keyInfo, out index) && (index - 1) >= 0 && (index - 1) <= servers.Length)
                {
                    index = index - 1;
                    Console.WriteLine("Removing server: {0}", servers[index]);
                    JobStorage.Current.GetConnection().RemoveServer(servers[index]);

                    Console.WriteLine("");
                    servers = GetServers();
                }

                keyInfo = Console.ReadLine();
            }

        }

        static string[] GetServers()
        {
            Console.WriteLine("Enter the server number to remove:");

            var monitor = JobStorage.Current.GetMonitoringApi();            
            var servers = monitor.Servers().Select(s => s.Name).ToArray();
            for (int i = 0; i < servers.Length; i++)
            {
                Console.WriteLine("{0}:\t{1}", i + 1, servers[i]);
            }

            return servers;
        }
    }
}
