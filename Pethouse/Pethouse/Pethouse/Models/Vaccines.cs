using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Pethouse.Models
{
    internal class Vaccines
    {
        public string Vacname { get; set; }
        public DateTime? VacDate { get; set; }
        public DateTime? VacExpDate { get; set; }
        public int PetId { get; set; }
        [JsonConstructor]
        public Vaccines(int petId, string vacname, DateTime? vacdate, DateTime? vacexpdate)
        {

            PetId = petId;
            VacDate = vacdate;
            VacExpDate = vacexpdate;

        }

        public Vaccines()
        {
        }

        public static IEnumerable<Vaccines> Get()
        {
            return new List<Vaccines> { };
        }
    
    }
}