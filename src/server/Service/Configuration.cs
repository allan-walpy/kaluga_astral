using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

using Hostel.Server.Models;

namespace Hostel.Server.Service
{
    public class ConfigurationService
    {

        public IConfiguration Config { get; }

        public ConfigurationService(IConfiguration config)
        {
            this.Config = config;
        }

    }

}