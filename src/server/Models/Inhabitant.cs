using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hostel.Server.Models
{

    [Table("Inhabitants")]
    public class Inhabitant : IRecord
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public Customer Customer { get; set; }
        public int CustomerId { get; set; }

        [Required]
        public Room Room { get; set; }
        public int RoomId { get; set; }

        [Required]
        public DateTime CheckIn { get; set; }

    }

}