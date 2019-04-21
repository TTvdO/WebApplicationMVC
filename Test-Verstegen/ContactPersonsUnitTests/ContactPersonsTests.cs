using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Website_Verstegen.Data;
using Website_Verstegen.Models;
using Website_Verstegen.Controllers;
using Xunit;
using Microsoft.AspNetCore.Mvc;

namespace Test_Verstegen.ContactPersonsUnitTests
{
    public class ContactPersonsTests
    {
        private readonly DatabaseContext _context;
        private string databaseName;

        List<ContactPerson> ContactPeople = new List<ContactPerson>()
        {
            new ContactPerson()
            { Id = 1,
              CountryId = 1,
              ProvinceId = 1,
              EmailAdres = "tspek9@gmail.com",
              Telefoonnummer = 0123456789,
              Functie = "Opperhoofd",
              Naam = "Tom",
              ImageUrl = "t",
              Country = new Country()
              {
                  Id = 1,
                  Naam = "Nederland",
                  Code = "NL"
              },
              Province = new Province()
              {
                    Id = 1,
                    CountryId = 1,
                    Naam = "Zuid-Holland"
              }
            },
            new ContactPerson()
            {
                Id = 2,
                CountryId = 2,
                EmailAdres = "16103815@student.hhs.nl",
                Telefoonnummer = 0123456678,
                ImageUrl = "t",
                Functie = "ICT-er",
                Naam = "Tim",
                Country = new Country()
                {
                    Id = 2,
                    Naam = "Belgie",
                    Code = "BE"
                },
                Province = new Province()
                {
                    Id = 2,
                    CountryId = 2,
                    Naam = "Vlaanderen",
                }
            }
        };

        private DatabaseContext GetInMemoryDbMetData()
        {
            var context = GetNewInMemoryDatabase(true);
            ContactPeople.ForEach(p => context.Add(p));
            context.SaveChanges();
            return GetNewInMemoryDatabase(false);
        }

        private DatabaseContext GetNewInMemoryDatabase(bool NewDb)
        {
            if (NewDb) this.databaseName = Guid.NewGuid().ToString();
            var options = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(this.databaseName)
                .Options;

            return new DatabaseContext(options);
        }

        [Fact]
        public async Task ContactPersonIndex()
        {
            var context = GetInMemoryDbMetData();
            var controller = new ContactPersonsController(context);

            var result = await controller.Index();
            var viewResult = Assert.IsAssignableFrom<ViewResult>(result);

            var model = Assert.IsAssignableFrom<List<ContactPerson>>(viewResult.ViewData.Model);


            Assert.Equal(2, model.Count);
            Assert.Equal("Tom", model[0].Naam);
            Assert.Equal("Nederland", model[0].Country.Naam);
            Assert.Equal("Vlaanderen", model[1].Province.Naam);
            Assert.NotNull(viewResult);
        }

        [Fact]
        public async Task ContactPersonsCreate()
        {
            var context = GetNewInMemoryDatabase(true);
            var controller = new ContactPersonsController(context);

            var result = await controller.Create(ContactPeople[0]);
            var viewResult = Assert.IsType<RedirectToActionResult>(result);

            Assert.Equal("Index", viewResult.ActionName);
        }

        [Fact]
        public async Task EditContactPersonTest()
        {
            var context = GetInMemoryDbMetData();
            var controller = new ContactPersonsController(context);

            var viewResult = await controller.Edit(ContactPeople[0].Id, ContactPeople[0]);
            var model = Assert.IsType<RedirectToActionResult>(viewResult);

            Assert.Equal("Index", model.ActionName);
        }

        [Fact]
        public async Task DeleteContactPersonTest()
        {
            var context = GetInMemoryDbMetData();
            var controller = new ContactPersonsController(context);

            var viewResult = await controller.DeleteConfirmed(ContactPeople[0].Id);
            var model = Assert.IsAssignableFrom<RedirectToActionResult>(viewResult);

            Assert.Equal("Index", model.ActionName);
        }

        [Fact]
        public async Task DetailsContactPersonTest()
        {
            var context = GetInMemoryDbMetData();
            var controller = new ContactPersonsController(context);

            var model = Assert.IsAssignableFrom<ContactPerson>(Assert.IsAssignableFrom<ViewResult>(await controller.Details(ContactPeople[0].Id)).ViewData.Model);

            Assert.Equal(1, ContactPeople[0].Id);
            Assert.Equal(1, ContactPeople[0].CountryId);
            Assert.Equal("tspek9@gmail.com", ContactPeople[0].EmailAdres);
            Assert.Equal("Opperhoofd", ContactPeople[0].Functie);
            Assert.Equal("Tom", ContactPeople[0].Naam);
            Assert.Equal(1, ContactPeople[0].Country.Id);
            Assert.Equal("Nederland", ContactPeople[0].Country.Naam);
            Assert.Equal("NL", ContactPeople[0].Country.Code);
            Assert.Equal(1, ContactPeople[0].Province.Id);
            Assert.Equal(1, ContactPeople[0].Province.CountryId);
            Assert.Equal("Zuid-Holland", ContactPeople[0].Province.Naam);
        }
    }
}
