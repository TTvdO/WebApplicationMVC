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
    public class ProvincesUnitTests
    {
        private string databaseName;

        public List<Province> provinces = new List<Province>()
        {
            new Province()
            {
                CountryId = 1,
                Naam = "Noord-Holland",
                Id = 1
            },
            new Province()
            {
                CountryId = 2,
                Naam = "Zuid-Holland",
                Id = 2
            }
        };

        private DatabaseContext GetInMemoryDbMetData()
        {
            var context = GetNewInMemoryDatabase(true);
            provinces.ForEach(p => context.Add(p));
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
            var controller = new ProvincesController(context);

            var result = await controller.Index();
            var viewResult = Assert.IsAssignableFrom<ViewResult>(result);
            var model = Assert.IsAssignableFrom<List<Province>>(viewResult.ViewData.Model);

            Assert.Equal(2, model.Count);
            Assert.Equal("Zuid-Holland", model[1].Naam);
        }

        [Fact]
        public async Task CreateProvince()
        {
            var context = GetNewInMemoryDatabase(true);
            var controller = new ProvincesController(context);

            var result = await controller.Create(provinces[0]);
            var viewResult = Assert.IsType<RedirectToActionResult>(result);

            Assert.Equal("Index", viewResult.ActionName);
        }

        [Fact]
        public async Task EditProvince()
        {
            var context = GetInMemoryDbMetData();
            var controller = new ProvincesController(context);

            var viewResult = await controller.Edit(provinces[0].Id, provinces[0]);
            var model = Assert.IsType<RedirectToActionResult>(viewResult);

            Assert.Equal("Index", model.ActionName);
        }

        [Fact]
        public async Task DeleteProvince()
        {
            var context = GetInMemoryDbMetData();
            var controller = new ProvincesController(context);

            var viewResult = await controller.DeleteConfirmed(provinces[0].Id);
            var model = Assert.IsAssignableFrom<RedirectToActionResult>(viewResult);

            Assert.Equal("Index", model.ActionName);
        }

        [Fact]
        public async Task DetailsProvince()
        {
            var context = GetInMemoryDbMetData();
            var controller = new ProvincesController(context);

            Province c = await context.Counties.FirstOrDefaultAsync();
            var model = Assert.IsAssignableFrom<List<Province>>(Assert.IsAssignableFrom<ViewResult>(await controller.Index()).ViewData.Model);

            Assert.Equal("Noord-Holland", model[0].Naam);
            Assert.Equal(1, model[0].Id);
            Assert.Equal(1, model[0].CountryId);
        }


    }
}
