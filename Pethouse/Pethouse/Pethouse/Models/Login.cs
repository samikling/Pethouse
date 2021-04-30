using System.Collections.Generic;

namespace Pethouse.Models
{
    public class Login
    {
        public int? UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PassWord { get; set; }
        public virtual ICollection<Pets> Pets { get; set; }

    }
}
