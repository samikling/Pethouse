using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Pethouse.Models
{
    public  class Pets
    {
        public  int PetId { get; set; }
        public  string Petname { get; set; }
        public  DateTime? Birthdate { get; set; }
        public  string Photo { get; set; }
        public  int UserId { get; set; }
        public  int? RaceId { get; set; }
        public  int? BreedId { get; set; }
        public string Breed { get; set; }
        public string Race { get; set; }
        public int? User { get; set; }
        [JsonConstructor]
        public Pets(int petId, string petname, DateTime? birthdate, string photo, int userId, int? raceId, int? breedId, string breed, string race, int? user)
        {

            PetId = petId;
            Petname = petname;
            Birthdate = birthdate;
            Photo = photo;
            UserId = userId;
            RaceId = raceId;
            BreedId = breedId;
            Breed = breed;
            Race = race;
            User = user;

        }

        public Pets()
        {
        }

        public static IEnumerable<Pets> Get()
        {
            return new List<Pets> { };
        }
    }
}
