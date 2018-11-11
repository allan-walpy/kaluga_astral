using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

using Hostel.Server.Model;
using Hostel.Server.Service;

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

    }
}