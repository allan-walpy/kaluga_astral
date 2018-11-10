using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hostel.Server.Model
{

    [Table("Rooms")]
    public class Room
    {


        [Key]
        public int Id;

        [Required]
        public int Number;

        public int Capacity;

        public RoomCategory Category;

        public Inhabitant Inhabitant;

    }

}