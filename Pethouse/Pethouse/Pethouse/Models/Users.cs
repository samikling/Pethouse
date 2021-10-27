using System.Collections.Generic;

namespace Pethouse.Models
{
    internal class Users
    {
        public Users()
        {
            Pets = new HashSet<Pets>();
        }

        public int UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public virtual ICollection<Pets> Pets { get; set; }
    }
}

