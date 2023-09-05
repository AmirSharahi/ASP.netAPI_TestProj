using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace EfCore6Test.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        
        [Required]
        [MaxLength(20)]
        public string Username { get; set; }
        
        [Required]
        [MaxLength(20)]
        public string Password { get; set; }
        public DateTime CreationTime { get; set; }
        [AllowNull]
        public DateTime LastLoginTime { get; set; }
        [Required]
        public int RoleId { get; set; }

        public Role Role { get; set; }


        public User()
        {
            
        }

        public User(UserInsert userInsert)
        {
            Username = userInsert.Username;
            Password = userInsert.Password;
            RoleId = userInsert.RoleId;
            CreationTime = DateTime.Now;
        }
    }
}
