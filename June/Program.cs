using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace June
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)


             .ConfigureLogging((hostingContext, logging) =>
             {
                 logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                 logging.AddConsole();
                 logging.AddDebug();
                 logging.AddNLog();
                 logging.AddEventSourceLogger();
                 // Enable NLog as one of the Logging Provider
                 
             })

                //.ConfigureWebHostDefaults(webBuilder =>
                //{
                    .UseStartup<Startup>();
                
                
                
                
                //});
    }

}
