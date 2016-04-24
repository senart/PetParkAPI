using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Net.Http;
using System.Configuration;
using System.IO;
using System.Data.Entity;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using PetPark.Interfaces;
using PetPark.Models;

namespace PetPark.Implementations
{
    public class FileService : IFileService
    {
        public async Task<List<FileModel>> UploadFiles(HttpContent httpContent)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var blobUploadProvider = new BlobStorageUploadProvider();

                var list = await httpContent.ReadAsMultipartAsync(blobUploadProvider)
                    .ContinueWith(task =>
                    {
                        if (task.IsFaulted || task.IsCanceled)
                        {
                            throw task.Exception;
                        }

                        var provider = task.Result;
                        return provider.Uploads.ToList();
                    });

                foreach (FileModel file in list)
                {
                    // IF file exists
                    if (db.Files.Count(e => e.FileUrl == file.FileUrl) > 0)
                    {
                        db.Entry(file).State = EntityState.Modified;
                    }
                    else
                    {
                        db.Files.Add(file);
                    }
                }
                db.SaveChanges();

                return list;
            }
        }

        public async Task<FileModel> UploadSingleFile(HttpContent httpContent)
        {
            var list = await this.UploadFiles(httpContent);

            if (list.Count > 0) return list[0];
            else return null;
        }

        public bool DeleteFile(string url)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                FileModel file = db.Files.Find(url);
                if (file == null) 
                {
                    return false;
                }

                var container = BlobHelper.GetBlobContainer();
                var blob = container.GetBlockBlobReference(file.FileName);
                blob.Delete();
                
                db.Files.Remove(file);
                db.SaveChanges();

                return true;
            }
        }

        //public async Task<BlobDownloadModel> DownloadBlob(int blobId)
        //{
        //    // TODO: You must implement this helper method. It should retrieve blob info
        //    // from your database, based on the blobId. The record should contain the
        //    // blobName, which you should return as the result of this helper method.
        //    var blobName = "GGGGGGGGG"; //GetBlobName(blobId);

        //    if (!String.IsNullOrEmpty(blobName))
        //    {
        //        var container = BlobHelper.GetBlobContainer();
        //        var blob = container.GetBlockBlobReference(blobName);

        //        // Download the blob into a memory stream. Notice that we're not putting the memory
        //        // stream in a using statement. This is because we need the stream to be open for the
        //        // API controller in order for the file to actually be downloadable. The closing and
        //        // disposing of the stream is handled by the Web API framework.
        //        var ms = new MemoryStream();
        //        await blob.DownloadToStreamAsync(ms);

        //        // Strip off any folder structure so the file name is just the file name
        //        var lastPos = blob.Name.LastIndexOf('/');
        //        var fileName = blob.Name.Substring(lastPos + 1, blob.Name.Length - lastPos - 1);

        //        // Build and return the download model with the blob stream and its relevant info
        //        var download = new BlobDownloadModel
        //        {
        //            BlobStream = ms,
        //            BlobFileName = fileName,
        //            BlobLength = blob.Properties.Length,
        //            BlobContentType = blob.Properties.ContentType
        //        };

        //        return download;
        //    }

        //    // Otherwise
        //    return null;
        //}
    }

    public class BlobStorageUploadProvider : MultipartFileStreamProvider
    {
        public List<FileModel> Uploads { get; set; }

        public BlobStorageUploadProvider()
            : base(Path.GetTempPath())
        {
            Uploads = new List<FileModel>();
        }

        public override Task ExecutePostProcessingAsync()
        {
            // NOTE: FileData is a property of MultipartFileStreamProvider and is a list of multipart
            // files that have been uploaded and saved to disk in the Path.GetTempPath() location.

            foreach (var fileData in FileData)
            {
                // Sometimes the filename has a leading and trailing double-quote character
                // when uploaded, so we trim it; otherwise, we get an illegal character exception
                var fileName = Guid.NewGuid().ToString().GetHashCode().ToString("x") + Guid.NewGuid().ToString().GetHashCode().ToString("x") + ".bmp";
                //var fileName = Path.GetFileName(fileData.Headers.ContentDisposition.FileName.Trim('"'));

                // Retrieve reference to a blob
                var blobContainer = BlobHelper.GetBlobContainer();
                var blob = blobContainer.GetBlockBlobReference(fileName);

                // Set the blob content type
                blob.Properties.ContentType = fileData.Headers.ContentType.MediaType;

                // Upload file into blob storage, basically copying it from local disk into Azure
                using (var fs = File.OpenRead(fileData.LocalFileName))
                {
                    blob.UploadFromStream(fs);
                }

                // Delete local file from disk
                File.Delete(fileData.LocalFileName);

                // Create blob upload model with properties from blob info
                var blobUpload = new FileModel
                {
                    FileName = blob.Name,
                    FileUrl = blob.Uri.AbsoluteUri,
                    FileSizeInBytes = blob.Properties.Length
                };

                // Add uploaded blob to the list
                Uploads.Add(blobUpload);
            }

            return base.ExecutePostProcessingAsync();
        }
    }

    public static class BlobHelper
    {
        public static CloudBlobContainer GetBlobContainer()
        {
            // Pull these from config
            var blobStorageConnectionString = ConfigurationManager.AppSettings["BlobStorageConnectionString"];
            var blobStorageContainerName = ConfigurationManager.AppSettings["BlobStorageContainerName"];

            // Create blob client and return reference to the container
            var blobStorageAccount = CloudStorageAccount.Parse(blobStorageConnectionString);
            var blobClient = blobStorageAccount.CreateCloudBlobClient();
            return blobClient.GetContainerReference(blobStorageContainerName);
        }
    }
}