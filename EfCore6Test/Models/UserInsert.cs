using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EfCore6Test.Models
{
    [NotMapped]
    public class UserInsert
    {
        [Required]
        [MaxLength(20)]
        public string Username { get; set; }

        [Required]
        [MaxLength(20)]
        public string Password { get; set; }
        public int RoleId { get; set; }
    }
}
