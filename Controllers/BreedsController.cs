using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using PetPark.Models;

namespace PetPark.Controllers
{
    [Authorize]
    public class BreedsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Breeds/Poodle
        [ResponseType(typeof(Breed))]
        public IEnumerable<Breed> GetBreed(string id)
        {
            return db.Breeds.Where(b=>b.Species == id);
        }
    }
}