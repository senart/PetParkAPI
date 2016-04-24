using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PetPark.Models
{
    public class PetImage
    {
        public int PetImageID { get; set; }
        [Required]
        public int PetID { get; set; }
        [Required]
        public string ImageURL { get; set; }
    }

    public class AttatchProfileImageBindingModel
    {
        [Required]
        public int PetImageID { get; set; }
    }
}