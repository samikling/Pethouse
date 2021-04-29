using System;
using System.Collections.Generic;
using System.Text;

namespace Pethouse.Models
{
    class Pets
    {
        public int PetId { get; set; }
        public string Petname { get; set; }
        public DateTime? Birthdate { get; set; }
        public string Photo { get; set; }
        public int UserId { get; set; }
        public int? RaceId { get; set; }
        public int? BreedId { get; set; }    
    }
}
