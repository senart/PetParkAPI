using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PetPark.Models;

namespace PetPark.Interfaces
{
    public interface IPetService
    {
        List<PetLocationDTO> GetPetsForUser(string userID);

        List<PetLocationDTO> GetAllPets();

        Pet GetPet(int petID);

        PetImage AddPetImage(int petID, PetImage image);

        List<PetLocationDTO> GetAllBySpecies(String species);

        IEnumerable<PetImage> GetPetImages(int petID);

        bool AttatchProfileImage(int petID, int petImageID);

        void CreatePet(Pet pet);

        void EditPet(Pet pet);

        bool DeletePet(int petID);
    }
}