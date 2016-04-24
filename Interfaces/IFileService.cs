using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Threading.Tasks;
using PetPark.Models;

namespace PetPark.Interfaces
{
    public interface IFileService
    {
        Task<List<FileModel>> UploadFiles(HttpContent httpContent);
        Task<FileModel> UploadSingleFile(HttpContent httpContent);
        bool DeleteFile(string url);
    }
}