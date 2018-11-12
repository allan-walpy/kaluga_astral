using System;
using Microsoft.EntityFrameworkCore;

using Hostel.Server.Models;

namespace Hostel.Server.Services
{
    public class DatabaseService
    {

        private String _connectionString;

        public ApplicationContext Context
        {
            get
            {
                var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>();

                var options = optionsBuilder
                    .UseMySQL(_connectionString)
                    .Options;
                return new ApplicationContext(options);
            }
        }

        public DatabaseService(String connectionString)
        {
            this._connectionString = connectionString;
        }

    }

}