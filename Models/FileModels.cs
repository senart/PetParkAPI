using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PetPark.Models
{
    public class FileModel
    {
        [Key]
        public string FileUrl { get; set; }
        [Required]
        public string FileName { get; set; }
        [Required]
        public long FileSizeInBytes { get; set; }
    }

    public class FileDeleteModel
    {
        [Required]
        public string FileUrl { get; set; }
    }
}