using System;

namespace Pethouse.Models
{
    internal class Medications
    {
        public int MedId { get; set; }
        public string Medname { get; set; }
        public DateTime? MedDate { get; set; }
        public DateTime? MedExpDate { get; set; }
        public int PetId { get; set; }
    }
}