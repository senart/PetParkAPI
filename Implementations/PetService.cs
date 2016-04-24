using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Web;
using PetPark.Interfaces;
using PetPark.Models;

namespace PetPark.Implementations
{
    public class PetService : IPetService
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public List<PetLocationDTO> GetPetsForUser(string userID)
        {
            var pets = db.Pets.Include(p => p.PetImage);
            var users = db.Users;

            var query =
                from pet in pets
                join user in users
                on pet.UserID equals user.Id
                where user.Id == userID
                select new
                {
                    PetID = pet.PetID,
                    Species = pet.Species,
                    Breed = pet.Breed,
                    Gender = pet.Gender,
                    Name = pet.Name,
                    Age = pet.Age,
                    Weight = pet.Weight,
                    ProfilePic = pet.PetImage == null ? null : pet.PetImage.ImageURL,
                    Latitude = user.Latitude,
                    Longitude = user.Longitude
                };

            List<PetLocationDTO> newPets = new List<PetLocationDTO>();
            foreach (var pet in query)
            {
                newPets.Add(new PetLocationDTO
                {
                    PetID = pet.PetID,
                    Species = pet.Species,
                    Breed = pet.Breed,
                    Gender = pet.Gender,
                    Name = pet.Name,
                    Age = pet.Age,
                    Weight = pet.Weight,
                    ProfilePic = pet.ProfilePic,
                    Latitude = pet.Latitude,
                    Longitude = pet.Longitude
                });
            }

            return newPets;
        }

        public List<PetLocationDTO> GetAllPets()
        {
            var pets = db.Pets.Include(p => p.PetImage);
            var users = db.Users;
            var query =
                from pet in pets
                join user in users
                on pet.UserID equals user.Id
                select new
                {
                    PetID = pet.PetID,
                    Species = pet.Species,
                    Breed = pet.Breed,
                    Gender = pet.Gender,
                    Name = pet.Name,
                    Age = pet.Age,
                    Weight = pet.Weight,
                    ProfilePic = pet.PetImage == null ? null : pet.PetImage.ImageURL,
                    Latitude = user.Latitude,
                    Longitude = user.Longitude
                };

            List<PetLocationDTO> newPets = new List<PetLocationDTO>();

            foreach (var pet in query)
            {
                newPets.Add(new PetLocationDTO
                {
                    PetID = pet.PetID,
                    Species = pet.Species,
                    Breed = pet.Breed,
                    Gender = pet.Gender,
                    Name = pet.Name,
                    Age = pet.Age,
                    Weight = pet.Weight,
                    ProfilePic = pet.ProfilePic,
                    Latitude = pet.Latitude,
                    Longitude = pet.Longitude
                });
            }

            return newPets;
        }

        public List<PetLocationDTO> GetAllBySpecies(String species)
        {
            var pets = db.Pets.Include(p=>p.PetImage);
            var users = db.Users;
            
            var query =
                from pet in pets
                join user in users
                on pet.UserID equals user.Id
                where pet.Species == species
                select new
                {
                    PetID = pet.PetID,
                    Species = pet.Species,
                    Breed = pet.Breed,
                    Gender = pet.Gender,
                    Name = pet.Name,
                    Age = pet.Age,
                    Weight = pet.Weight,
                    ProfilePic = pet.PetImage == null ? null : pet.PetImage.ImageURL,
                    Latitude = user.Latitude,
                    Longitude = user.Longitude
                };

            List<PetLocationDTO> newPets = new List<PetLocationDTO>();
            foreach (var pet in query)
            {
                newPets.Add(new PetLocationDTO
                {
                    PetID = pet.PetID,
                    Species = pet.Species,
                    Breed = pet.Breed,
                    Gender = pet.Gender,
                    Name = pet.Name,
                    Age = pet.Age,
                    Weight = pet.Weight,
                    ProfilePic = pet.ProfilePic,
                    Latitude = pet.Latitude,
                    Longitude = pet.Longitude
                });
            }

            return newPets;
        }

        public Pet GetPet(int petID)
        {
            Pet pet = db.Pets.Include(p => p.PetImage).SingleOrDefault(x => x.PetID == petID);
            return pet;
        }

        public PetImage AddPetImage(int petID, PetImage image)
        {
            db.PetImages.Add(image);
            db.SaveChanges();

            return image;
        }

        public IEnumerable<PetImage> GetPetImages(int petID)
        {
            return db.PetImages.Where(p => p.PetID == petID);
        }

        public bool AttatchProfileImage(int petID, int petImageID)
        {
            Pet pet = db.Pets.Find(petID);
            PetImage petImage = db.PetImages.Find(petImageID);
            if (petImage == null) return false;

            pet.PetImage = petImage;
            db.SaveChanges();

            return true;
        }

        public void CreatePet(Pet pet)
        {
            db.Pets.Add(pet);
            db.SaveChanges();
        }

        public bool DeletePet(int petID)
        {
            Pet pet = db.Pets.Find(petID);
            if (pet == null) return false;

            db.Pets.Remove(pet);
            db.SaveChanges();
            return true;
        }

        public void EditPet(Pet pet)
        {
            db.Entry(pet).State = EntityState.Modified;
            db.SaveChanges();
        }

        private bool PetExists(int id)
        {
            return db.Pets.Count(e => e.PetID == id) > 0;
        }
    }
}