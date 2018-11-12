using System;
using Microsoft.EntityFrameworkCore;

using Hostel.Server.Models;

namespace Hostel.Server.Services
{
    public class DatabaseService
    {

        public ApplicationContext Context { get; }

        public DatabaseService(String connectionString)
        {
            this.Context = this.ConnectToDatabase(connectionString);
        }

        private ApplicationContext ConnectToDatabase(string connectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>();

            var options = optionsBuilder
                .UseMySQL(connectionString)
                .Options;
            return new ApplicationContext(options);
        }

    }

}