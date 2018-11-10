using System;
using Microsoft.EntityFrameworkCore;

using Hostel.Server.Model;

namespace Hostel.Server.Service
{
    public class DatabaseService
    {

        private String _connection;

        public ApplicationContext Context
        {
            get
            {
                var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>();

                var options = optionsBuilder
                    .UseMySQL(this._connection)
                    .Options;
                return new ApplicationContext(options);
            }
        }

        public DatabaseService(String connection)
        {
            this._connection = connection;
        }

    }

}