using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Hostel.Server
{
    public class Program
    {

        public static IConfiguration Config { get; private set; }


        public static void Main(string[] args)
        {
            ReadConfiguration(args);
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();

        public static void ReadConfiguration(string[] ags)
        {
            var builder = new ConfigurationBuilder();
            builder.SetBasePath(Directory.GetCurrentDirectory());

            // TODO: magisk stringk; add to args and add to constants;
            builder.AddJsonFile("appsettings.json");

            Program.Config = builder.Build();
        }

    }
}