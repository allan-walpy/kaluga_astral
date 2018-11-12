using System;
using Microsoft.Extensions.Configuration;

using Hostel.Server.Models;

namespace Hostel.Server
{

    public class Logic {


        public static double RoomPrice(IConfiguration config, RoomCategory category)
        {
            Console.WriteLine(category.ToString());
            return config.GetValue<double>($"price:{category.ToString()}");
        }

        public static double CalculatePrice(IConfiguration config, RoomCategory category, TimeSpan interval)
        {
            double hours = Math.Floor(interval.TotalHours) + 1;
            double roomPrice = RoomPrice(config, category);
            return roomPrice * hours;
        }

    }

}