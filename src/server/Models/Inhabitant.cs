using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hostel.Server.Model
{

    [Table("Inhabitants")]
    public class Inhabitant
    {
        [Key]
        public int Id;

        [Required]
        public Customer Customer;

        [Required]
        public Room Room;

        [Required]
        public DateTime CheckIn;

        public DateTime CheckOut;

    }

}