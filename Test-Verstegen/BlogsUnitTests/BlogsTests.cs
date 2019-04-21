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

namespace Test_Verstegen.BlogsUnitTests
{
    public class BlogsTests
    {
        private readonly DatabaseContext _context;
        private IHostingEnvironment _hostingEnvironment;
        private string databaseName;

        //Mocking Packagings To Test For This Unit Test
        List<Blog> Blogs = new List<Blog>()
        {
            new Blog{
                Id = 1,
                Title = "Winter 2018",
                Subtitle = "In the winter it snows sometimes, but sometimes it doesn't",
                Type = "Blog",
                Category = "Season",
                Content = "Snow is just SO beautiful: it covers everything like a fluffy white blanket and makes for a picturesque panorama.",
                ImageUrl = null
            },
            new Blog{
                Id = 2,
                Title = "Summer 2018",
                Subtitle = "In the summer the sun shines sometimes, but sometimes it doesn't",
                Type = "Blog",
                Category = "Season",
                Content = "Summer is the hottest of the four temperate seasons, falling after spring and before autumn.",
                ImageUrl = null
            }
        };

        //Gets The InMemory Database With The Mock Products
        private DatabaseContext GetInMemoryDbMetData()
        {
            var context = GetNewInMemoryDatabase(true);
            Blogs.ForEach(p => context.Add(p));
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
            var fileName = webRoothPath + "images\\Blogs\\ontbijt.jpg";
            fileMock.Setup(_ => _.FileName).Returns(fileName);
            return fileMock.Object;
        }

        [Fact]
        public async Task IndexBlogsController()
        {
            var context = GetInMemoryDbMetData();
            var controller = new BlogsController(context, _hostingEnvironment);

            var result = await controller.Index();
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<List<Blog>>(viewResult.ViewData.Model);
            
            //Test Blog 1
            Assert.Equal("Season", model[0].Category);
            Assert.Equal("Season", model[1].Category);

            //Test Blog 2
            Assert.Equal("Winter 2018", model[0].Title);
            Assert.Equal("Summer 2018", model[1].Title);
            Assert.Equal(2, model.Count);
            Assert.NotNull(viewResult);
        }

        [Fact]
        public async Task DetailsBlogsController()
        {
            var context = GetInMemoryDbMetData();
            var controller = new BlogsController(context, _hostingEnvironment);

            var result = await controller.Details(Blogs[1].Id);

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<Blog>(viewResult.ViewData.Model);

            Assert.Equal("Summer 2018", model.Title);
            Assert.Equal("Summer is the hottest of the four temperate seasons, falling after spring and before autumn.", model.Content);
            Assert.Equal("In the summer the sun shines sometimes, but sometimes it doesn't", model.Subtitle);
            Assert.NotNull(viewResult);
        }

        [Fact]
        public async Task PostCreateBlogsController()
        {
            //Getting Everything To Make This Unit Test Work
            IFormFile file = GetFormFile();
            IHostingEnvironment hostingEnvironment = GetHostingEnvironment();
            DatabaseContext context = GetInMemoryDbMetData();

            var controller = new BlogsController(context, hostingEnvironment);

            var result = await controller.Create(Blogs[0], file);
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", viewResult.ActionName);
        }

        [Fact]
        public async Task PostEditBlogsController()
        {
            //Getting Everything To Make This Unit Test Work
            IFormFile file = GetFormFile();
            IHostingEnvironment hostingEnvironment = GetHostingEnvironment();
            DatabaseContext context = GetInMemoryDbMetData();

            var controller = new BlogsController(context, hostingEnvironment);

            var result = await controller.Edit(Blogs[0].Id, Blogs[0], file);
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", viewResult.ActionName);
        }

        [Fact]
        public async Task DeleteBlogsController()
        {
            DatabaseContext context = GetInMemoryDbMetData();

            var controller = new BlogsController(context, _hostingEnvironment);

            var result = await controller.DeleteConfirmed(Blogs[0].Id);
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", viewResult.ActionName);
        }
    }
}
