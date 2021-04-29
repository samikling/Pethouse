using System;
using System.Collections.Generic;
using System.Text;

namespace Pethouse.Models
{
    class Vaccines
    {
        public string Vacname { get; set; }
        public DateTime? VacDate { get; set; }
        public DateTime? VacExpDate { get; set; }
        public int PetId { get; set; }
        
    }
}
