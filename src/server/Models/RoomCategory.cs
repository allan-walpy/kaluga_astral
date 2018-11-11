using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hostel.Server.Model
{

    public class RoomCategory
    {

        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }


        public List<Room> Rooms { get; set; }

    }

}