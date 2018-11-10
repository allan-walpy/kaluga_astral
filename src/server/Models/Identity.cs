using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Hostel.Server.Model
{

    [Table("Identities")]
    public class Identity
    {

        [Key]
        public string Login { get; set; }

        [Required]
        public string Hash { get; set; }

    }

}