using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

using Hostel.Server.Models;
using Hostel.Server.Services;

namespace Hostel.Server
{
    public static class Extensions
    {

        public static void UseDatabaseProvider(this IServiceCollection services, IConfiguration config)
        {
            // TODO: magick stringks here;
            services.AddSingleton<DatabaseService>(provider => new DatabaseService(
                config.GetConnectionString("LocalhostConection")
            ));
        }

        public static void UseConfigurationProvider(this IServiceCollection services)
        {
            services.AddSingleton<ConfigurationService>(provider => new ConfigurationService(Program.Config));
        }

        // thnx to https://gist.github.com/leggetter/769688
        /// <summary>
        /// read stream as string and outputs it;
        /// </summary>
        /// <param name="inputStream"></param>
        /// <returns></returns>
        public static string Stringify(this Stream inputStream)
        {
            string documentContents;
            using (Stream receiveStream = inputStream)
            {
                using (StreamReader readStream = new StreamReader(receiveStream, System.Text.Encoding.UTF8))
                {
                    documentContents = readStream.ReadToEnd();
                }
            }
            return documentContents;
        }

    }
}