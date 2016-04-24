using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using System.Threading.Tasks;
using PetPark.Models;
using PetPark.Interfaces;
using PetPark.Implementations;

namespace PetPark.Controllers
{
    [Authorize]
    public class FilesController : ApiController
    {
        // Interface in place so you can resolve with IoC container of your choice
        private readonly IFileService _fileService = new FileService();

        /// <summary>
        /// Uploads one or more blob files.
        /// </summary>
        /// <returns></returns>

        // POST: api/Files
        [ResponseType(typeof(List<FileModel>))]
        public async Task<IHttpActionResult> PostFile()
        {
            try
            {
                // This endpoint only supports multipart form data
                if (!Request.Content.IsMimeMultipartContent("form-data"))
                {
                    return StatusCode(HttpStatusCode.UnsupportedMediaType);
                }

                // Call service to perform upload, then check result to return as content
                var uploadedFiles = await _fileService.UploadFiles(Request.Content);
                if (uploadedFiles != null && uploadedFiles.Count > 0)
                {
                    return Ok(uploadedFiles);
                }

                // Otherwise
                return BadRequest();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // PUT: api/Files/FileDelete
        [HttpPut]
        [ResponseType(typeof(void))]
        public IHttpActionResult FileDelete(FileDeleteModel fileDeleteModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_fileService.DeleteFile(fileDeleteModel.FileUrl)) return NotFound();
            else return Ok();
        }
    }
}