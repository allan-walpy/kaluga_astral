using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hostel.Server.Model
{

    [Table("Inhabitants")]
    public class Inhabitant
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public Customer Customer { get; set; }

        [Required]
        public Room Room { get; set; }

        [Required]
        public DateTime CheckIn { get; set; }

        public DateTime CheckOut { get; set; }

    }

}