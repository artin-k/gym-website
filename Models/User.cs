
using System.ComponentModel.DataAnnotations.Schema;

namespace gymWebsite.Models
{
    public class User
    {
        public int Id { get; set; }

        [Column(TypeName = "varchar(255)")]
        public string Username { get; set; } = string.Empty;

        [Column(TypeName = "varchar(255)")]
        public string PasswordHash { get; set; } = string.Empty; // Or just "Password" if you prefer
    }
}
