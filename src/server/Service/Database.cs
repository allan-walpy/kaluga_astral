using System;

using Hostel.Server.Model;

namespace Hostel.Server.Service
{
    public class DatabaseService
    {

        public ApplicationContext Context { get; }

        public DatabaseService(ApplicationContext context)
        {
            this.Context = context;
        }

    }

}