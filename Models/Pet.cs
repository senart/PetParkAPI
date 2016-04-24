using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PetPark.Models
{
    public class Pet
    {
        public int PetID { get; set; }
        public string UserID { get; set; }
        [Required]
        public string Species { get; set; }
        [Required]
        public string Breed { get; set; }
        [Required]
        public string Gender { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int Age { get; set; }
        [Required]
        public double Weight { get; set; }
        public int? PetImageID { get; set; }
        public PetImage PetImage { get; set; }
    }

    public class PetDTO
    {
        public PetDTO(Pet pet)
        {
            PetID = pet.PetID;
            Species = pet.Species;
            Breed = pet.Breed;
            Gender = pet.Gender;
            Name = pet.Name;
            Age = pet.Age;
            Weight = pet.Weight;
            ProfilePic = pet.PetImage == null ? null : pet.PetImage.ImageURL;
        }
        public int PetID { get; set; }
        public string Species { get; set; }
        public string Breed { get; set; }
        public string Gender { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public double Weight { get; set; }
        public string ProfilePic { get; set; }
    }

    public class PetLocationDTO
    {
        public int PetID { get; set; }
        public string Species { get; set; }
        public string Breed { get; set; }
        public string Gender { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public double Weight { get; set; }
        public string ProfilePic { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; } 
    }
}