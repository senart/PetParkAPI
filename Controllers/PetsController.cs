using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.AspNet.Identity;
using PetPark.Models;
using PetPark.Implementations;
using PetPark.Interfaces;

namespace PetPark.Controllers
{
    [Authorize]
    [RoutePrefix("api/pets")]
    public class PetsController : ApiController
    {
        private IFileService _fileService = new FileService();
        private IPetService _petService = new PetService();

        // GET: api/Pets
        [Route("")]
        public IEnumerable<PetLocationDTO> GetPets()
        {
            return _petService.GetAllPets();
        }

        // GET: api/Pets/5
        [Route("{id:int}")]
        [ResponseType(typeof(Pet))]
        public IHttpActionResult GetPet(int id)
        {
            PetDTO petDTO = new PetDTO(_petService.GetPet(id));
            if (petDTO == null) return NotFound();

            return Ok(petDTO);
        }

        // GET: api/Pets/byspecies
        [Route("{species}/byspecies")]
        public IEnumerable<PetLocationDTO> GetBySpecies(String species)
        {
            return _petService.GetAllBySpecies(species);
        }

        // POST: api/Pets/5/UploadImage
        [Route("{id:int}/uploadImage")]
        public async Task<IHttpActionResult> UploadImage(int id)
        {
            // This endpoint only supports multipart form data
            if (!Request.Content.IsMimeMultipartContent("form-data"))
            {
                return StatusCode(HttpStatusCode.UnsupportedMediaType);
            }

            // Call service to perform upload, then attatch image url
            FileModel uploadedFile = await _fileService.UploadSingleFile(Request.Content);
            if (uploadedFile != null)
            {
                PetImage petImage = new PetImage
                {
                    PetID = id,
                    ImageURL = uploadedFile.FileUrl
                };

                _petService.AddPetImage(id, petImage);
                return Ok(petImage);
            }

            // Otherwise
            return BadRequest();
        }

        // GET: api/Pets/5/Images
        [Route("{id:int}/images")]
        public IEnumerable<PetImage> GetImages(int id)
        {
            return _petService.GetPetImages(id);
        }

        // POST: api/Pets/5/attatchProfileImage
        [Route("{id:int}/attatchProfileImage")]
        public IHttpActionResult AttatchProfileImage(int id, AttatchProfileImageBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (_petService.AttatchProfileImage(id, model.PetImageID)) return Ok();
            
            // Otherwise
            return InternalServerError(new Exception("No image with that id"));
        }

        // PUT: api/Pets/5
        [Route("{id:int}")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutPet(int id, Pet pet)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != pet.PetID)
            {
                return BadRequest();
            }
            
            _petService.EditPet(pet);

            return Ok();
        }

        // POST: api/Pets
        [Route("")]
        [ResponseType(typeof(Pet))]
        public IHttpActionResult PostPet(Pet pet)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            pet.UserID = User.Identity.GetUserId();
            _petService.CreatePet(pet);
            return Ok();
        }

        // DELETE: api/Pets/5
        [Route("{id:int}")]
        [ResponseType(typeof(Pet))]
        public IHttpActionResult DeletePet(int id)
        {
            if (_petService.DeletePet(id)) return Ok();
            else return NotFound();
        }
    }
}