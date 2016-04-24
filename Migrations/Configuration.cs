namespace PetPark.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using PetPark.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<PetPark.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(PetPark.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            
            //context.Pets.AddOrUpdate(
            //  p => p.Species,
            //  new Pet { Species = "Bunny", UserID = "851b83e9-36e3-4b84-b631-a01a0c7940ac", Breed = "Cute One", Gender = "Female", Name = "Wabbit", Age = 2, Weight = 2.3, PetImageID = 1 },
            //  new Pet {  Species = "Cat", UserID = "851b83e9-36e3-4b84-b631-a01a0c7940ac", Breed = "Small", Gender = "Male", Name = "Tom", Age = 1, Weight = 0.5 },
            //  new Pet { Species = "Dog", UserID = "851b83e9-36e3-4b84-b631-a01a0c7940ac", Breed = "Bulldog", Gender = "Male", Name = "Buck", Age = 2, Weight = 14.1 },
            //  new Pet { Species = "Turtule", UserID = "851b83e9-36e3-4b84-b631-a01a0c7940ac", Breed = "Some", Gender = "Male", Name = "Slow", Age = 2, Weight = 2.0 }
            //);
            
        }
    }
}
