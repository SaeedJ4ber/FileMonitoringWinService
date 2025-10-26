using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace FileMonitoringWinService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        //static void Main()
        //{
        //    ServiceBase[] ServicesToRun;
        //    ServicesToRun = new ServiceBase[] 
        //    { 
        //        new FileMonitoringWinService() 
        //    };
        //    ServiceBase.Run(ServicesToRun);
        //}

        static void Main(string[] args)
        {
            if (Environment.UserInteractive)
            {
                var service = new FileMonitoringWinService();
                service.StartInConsole();

                Console.WriteLine("Service running in console mode. Press any key to stop...");
                Console.ReadKey();

                service.StopInConsole();
            }
            else
            {
                ServiceBase.Run(new FileMonitoringWinService());
            }


        }
    }
}
