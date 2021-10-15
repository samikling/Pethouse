using System;

namespace Pethouse.Models
{
    internal class Grooming
    {
        public int GroomId { get; set; }
        public string Groomname { get; set; }
        public DateTime? GroomDate { get; set; }
        public DateTime? GroomExpDate { get; set; }
        public string Comments { get; set; }
        public int PetId { get; internal set; }
    }
}