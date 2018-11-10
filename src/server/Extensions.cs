using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

using Hostel.Server.Model;
using Hostel.Server.Service;

namespace Hostel.Server
{
    public static class Extensions
    {

        public static void UseDatabaseProvider(this IServiceCollection services)
        {
            var connectionString = "server=localhost;UserId=walpy;Password=2713;database=hostel;";
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>();

            var options = optionsBuilder
                .UseMySQL(connectionString)
                .Options;

            services.AddSingleton<DatabaseService>(provider =>
                new DatabaseService(
                    new ApplicationContext(options))
                    );
        }


    }
}