using Microsoft.EntityFrameworkCore;

using Hostel.Server.Models;
using Hostel.Server.Services;

namespace Hostel.Server.Database
{

    public class RoomTable : Table<Room>
    {

        public RoomTable(DatabaseService dbService)
        {
            this.DbService = dbService;
        }

        protected override DbSet<Room> GetDbSet(ApplicationContext db)
        {
            return db.Rooms;
        }

        public override bool IsRequiredFieldsSet(Room item)
        {
            return true;
        }

    }
}