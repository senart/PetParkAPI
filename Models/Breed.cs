using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PetPark.Models
{
    public class Breed
    {
        [Required]
        public string BreedID { get; set; }
        public string Species { get; set; }
    }
}