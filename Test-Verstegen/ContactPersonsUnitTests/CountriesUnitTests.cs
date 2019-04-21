using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Website_Verstegen.Controllers;
using Website_Verstegen.Data;
using Website_Verstegen.Models;
using Xunit;

namespace Test_Verstegen.ContactPersonsUnitTests
{
    public class CountriesUnitTests
    {
        private string databaseName;

        public List<Country> countries = new List<Country>()
        {
            new Country()
            {
                Code = "NL",
                Naam = "Nederland",
                Id = 1,
                People = new List<ContactPerson>()
                {
                   new ContactPerson()
                   { Id = 1,
                      CountryId = 1,
                      EmailAdres = "tspek9@gmail.com",
                      Functie = "Opperhoofd",
                      Naam = "Tom",
                      Province = new Province()
                      {
                            Id = 1,
                            CountryId = 1,
                            Naam = "Zuid-Holland"
                      }
                   }
                }
            },
            new Country()
            {
                Code = "DE",
                Naam = "Duitsland",
                Id = 2,
                People = new List<ContactPerson>()
                {
                    new ContactPerson()
                    {
                        CountryId = 2,
                        Id = 2,
                        EmailAdres = "16103815@hhs.nl",
                        Naam = "Nathan",
                        Functie = "ICT-er",
                        Province = new Province()
                        {
                            Id = 2,
                            Naam = "Noord-Holland",
                            CountryId = 2
                        }
                    }
                }
            }
        };
        
        private DatabaseContext GetInMemoryDbMetData()
        {
            var context = GetNewInMemoryDatabase(true);
            countries.ForEach(p => context.Add(p));
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
        public async Task IndexCountries()
        {
            var context = GetInMemoryDbMetData();
            var controller = new CountriesController(context);

            var result = await controller.Index();
            var viewResult = Assert.IsAssignableFrom<ViewResult>(result);
            var model = Assert.IsAssignableFrom<List<Country>>(viewResult.ViewData.Model);

            Assert.Equal(2, model.Count);
            Assert.Equal("Duitsland", model[1].Naam);
        }

        [Fact]
        public async Task CreateCountries()
        {
            var context = GetNewInMemoryDatabase(true);
            var controller = new CountriesController(context);

            var result = await controller.Create(countries[0]);
            var viewResult = Assert.IsType<RedirectToActionResult>(result);

            Assert.Equal("Index", viewResult.ActionName);

        }

        [Fact]
        public async Task EditCountries()
        {
            var context = GetInMemoryDbMetData();
            var controller = new CountriesController(context);

            var viewResult = await controller.Edit(countries[0].Id, countries[0]);
            var model = Assert.IsType<RedirectToActionResult>(viewResult);

            Assert.Equal("Index", model.ActionName);

        }

        [Fact]
        public async Task DeleteCountry()
        {
            var context = GetInMemoryDbMetData();
            var controller = new CountriesController(context);

            var result = await controller.DeleteConfirmed(countries[0].Id);
            var view = Assert.IsAssignableFrom<RedirectToActionResult>(result);
            Assert.Equal("Index", view.ActionName);
        }

        [Fact]
        public async Task DetailsCountry()
        {
            var context = GetInMemoryDbMetData();
            var controller = new CountriesController(context);

            var result = await controller.Details(countries[0].Id);
            var viewResult = Assert.IsAssignableFrom<ViewResult>(result);

            Assert.Equal("NL", countries[0].Code);
            Assert.Equal("Nederland", countries[0].Naam);
            Assert.Equal(1, countries[0].Id);
        }
    }
}
