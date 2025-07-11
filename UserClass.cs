using System.ComponentModel.DataAnnotations.Schema;

namespace gymWebsite
{
    public class UserClass
    {
        public int Id { get; set; }

        public string Username { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;
    }
}
