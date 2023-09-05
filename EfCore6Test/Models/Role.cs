using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EfCore6Test.Data;

namespace EfCore6Test.Models
{
    public class Role
    {
        [Key]
        public int RoleId { get; set; }

        [Required]
        [MaxLength(10)]
        public string RoleTitle { get; set; }

        public IEnumerable<User> Users { get; set;}

        public Role()
        {
        }

        public Role(string roleTitle)
        {
            RoleTitle = roleTitle;
        }
    }
}
