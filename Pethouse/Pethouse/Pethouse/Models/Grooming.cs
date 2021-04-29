using System;
using System.Collections.Generic;
using System.Text;

namespace Pethouse.Models
{
    class Grooming
    {
        public String Groomname { get; set; }
        public DateTime? GroomDate { get; set; }
        public DateTime? GroomExpDate { get; set; }
        public string Comments { get; set; }
    }
}
