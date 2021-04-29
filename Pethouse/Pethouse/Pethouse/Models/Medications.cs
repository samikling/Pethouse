using System;
using System.Collections.Generic;
using System.Text;

namespace Pethouse.Models
{
    class Medications
    {
        public string Medname { get; set; }
        public DateTime? MedDate { get; set; }
        public DateTime? MedExpDate { get; set; }
        public int PetId { get; set; }

    }
}
