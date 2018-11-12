using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hostel.Server.Models
{

    [Table("Customers")]
    public class Customer
    {

        [Key]
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string SecondName { get; set; }

        public string ThirdName { get; set; }

        public Inhabitant Inhabitant { get; set; }

        public int? InhabitantId { get; set; }

    }

}