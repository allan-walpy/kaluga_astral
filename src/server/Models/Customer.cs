using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hostel.Server.Model
{

    [Table("Customers")]
    public class Customer
    {

        [Key]
        public int Id;

        [Required]
        public string FirstName;

        [Required]
        public string SecondName;

        public string ThirdName;

    }

}