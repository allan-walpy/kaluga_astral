using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hostel.Server.Model
{

    [Table("Rooms")]
    public class Room
    {


        [Key]
        public int Id { get; set; }

        [Required]
        public int Number { get; set; }

        public int Capacity { get; set; }

        public RoomCategory Category { get; set; }

        public Inhabitant Inhabitant { get; set; }

    }

}