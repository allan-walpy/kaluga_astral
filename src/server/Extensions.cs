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
            // TODO: magick stringks here;
            services.AddSingleton<DatabaseService>(provider => new DatabaseService("server=localhost;UserId=walpy;Password=2713;database=hostel;"));
        }

    }
}