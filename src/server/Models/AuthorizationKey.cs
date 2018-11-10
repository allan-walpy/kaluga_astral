using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hostel.Server.Model
{

    [Table("Authorization")]
    public class AuthKey
    {

        [Key]
        public string Login;

        [Required]
        public string Hash;

    }

}