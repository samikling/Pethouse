using System;

namespace Pethouse.Models
{
    internal class Vaccines
    {
        public string Vacname { get; set; }
        public DateTime? VacDate { get; set; }
        public DateTime? VacExpDate { get; set; }
        public int PetId { get; set; }
    }
}