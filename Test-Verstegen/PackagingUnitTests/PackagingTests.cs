using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Website_Verstegen.Controllers;
using Website_Verstegen.Data;
using Website_Verstegen.Models;
using Xunit;

namespace Test_Verstegen.PackagingUnitTests
{
    public class BlogsTests
    {
        private readonly DatabaseContext _context;
        private IHostingEnvironment _hostingEnvironment;
        private string databaseName;

        //Mocking Packagings To Test For This Unit Test
        List<Packaging> packagings = new List<Packaging>()
        {
            new Packaging{
                Id = 1,
                Title = "Big Packaging",
                Type = "Packaging",
                Category = "Fast Food",
                Description = "Big packaging for fast food restaurants",
                ImageUrl = null,
                AltText = "Big fast food packaging",
                Contents = "500ml",
                PackagingHeight = 50,
                PackagingWidth = 50
            },
            new Packaging{
                Id = 2,
                Title = "Small Packaging",
                Type = "Packaging",
                Category = "Vegan Food",
                Description = "Small packaging for vegan restaurants",
                ImageUrl = null,
                AltText = "Small vegan food packaging",
                Contents = "300ml",
                PackagingHeight = 70,
                PackagingWidth = 70
            }
        };

        //Gets The InMemory Database With The Mock Products
        private DatabaseContext GetInMemoryDbMetData()
        {
            var context = GetNewInMemoryDatabase(true);
            packagings.ForEach(p => context.Add(p));
            context.SaveChanges();
            return GetNewInMemoryDatabase(false);
        }

        //Refreshes The InMemory Database After Every Test Run
        private DatabaseContext GetNewInMemoryDatabase(bool NewDb)
        {
            if (NewDb) this.databaseName = Guid.NewGuid().ToString();
            var options = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(this.databaseName)
                .Options;

            return new DatabaseContext(options);
        }

        //Mocking a IHostingEnvironment Object + Get Local Path
        private IHostingEnvironment GetHostingEnvironment()
        {
            var path = Directory.GetCurrentDirectory();
            var url = path.Split("Test-Verstegen")[0];
            var webRoothPath = url + "Website-Verstegen\\" + "wwwroot\\";
            var mockEnvironment = new Mock<IHostingEnvironment>();
            mockEnvironment.Setup(m => m.WebRootPath)
                .Returns(webRoothPath);

            return mockEnvironment.Object;
        }

        //Mocking an IFormFile Object
        private IFormFile GetFormFile()
        {
            var path = Directory.GetCurrentDirectory();
            var url = path.Split("Test-Verstegen")[0];
            var webRoothPath = url + "Website-Verstegen\\" + "wwwroot\\";
            var fileMock = new Mock<IFormFile>();
            var fileName = webRoothPath + "images\\Packagings\\ontbijt.jpg";
            fileMock.Setup(_ => _.FileName).Returns(fileName);
            return fileMock.Object;
        }

        [Fact]
        public async Task IndexPackagingsController()
        {
            var context = GetInMemoryDbMetData();
            var controller = new PackagingsController(context, _hostingEnvironment);

            var result = await controller.Index();
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<List<Packaging>>(viewResult.ViewData.Model);
            
            //Test Packaging 1
            Assert.Equal(50, model[0].PackagingHeight);
            Assert.Equal(70, model[1].PackagingHeight);

            //Test Packaging 2
            Assert.Equal("Big Packaging", model[0].Title);
            Assert.Equal("Small Packaging", model[1].Title);
            Assert.Equal(2, model.Count);
            Assert.NotNull(viewResult);
        }

        [Fact]
        public async Task DetailsPackagingsController()
        {
            var context = GetInMemoryDbMetData();
            var controller = new PackagingsController(context, _hostingEnvironment);

            var result = await controller.Details(packagings[0].Id);

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<Packaging>(viewResult.ViewData.Model);

            Assert.Equal("500ml", model.Contents);
            Assert.Equal("Fast Food", model.Category);
            Assert.Equal(50, model.PackagingWidth);
            Assert.NotNull(viewResult);
        }

        [Fact]
        public async Task PostCreatePackagingController()
        {
            //Getting Everything To Make This Unit Test Work
            IFormFile file = GetFormFile();
            IHostingEnvironment hostingEnvironment = GetHostingEnvironment();
            DatabaseContext context = GetNewInMemoryDatabase(true);

            var controller = new PackagingsController(context, hostingEnvironment);

            var result = await controller.Create(packagings[0], file);
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", viewResult.ActionName);
        }

        [Fact]
        public async Task PostEditPackagingController()
        {
            //Getting Everything To Make This Unit Test Work
            IFormFile file = GetFormFile();
            IHostingEnvironment hostingEnvironment = GetHostingEnvironment();
            DatabaseContext context = GetInMemoryDbMetData();

            var controller = new PackagingsController(context, hostingEnvironment);

            var result = await controller.Edit(packagings[0].Id, packagings[0], file);
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", viewResult.ActionName);
        }

        [Fact]
        public async Task DeletePackagingController()
        {
            DatabaseContext context = GetInMemoryDbMetData();

            var controller = new PackagingsController(context, _hostingEnvironment);

            var result = await controller.DeleteConfirmed(packagings[0].Id);
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", viewResult.ActionName);
        }
    }
}
