using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace PetPark.Models.Blob
{
    public class BlobDownloadModel
    {
        public MemoryStream BlobStream { get; set; }
        public string BlobFileName { get; set; }
        public string BlobContentType { get; set; }
        public long BlobLength { get; set; }
    }
}