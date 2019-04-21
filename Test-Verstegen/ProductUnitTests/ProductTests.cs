using System;
using Xunit;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using System.Threading.Tasks;
using Website_Verstegen.Controllers;
using Website_Verstegen.Data;
using Website_Verstegen.Models;
using Microsoft.AspNetCore.Http;
using Moq;
using System.IO;
using Website_Verstegen.ViewModels;

namespace Test_Verstegen.ProductUnitTests
{
    public class ProductTests
    {
        private readonly DatabaseContext _context;
        private IHostingEnvironment _hostingEnvironment;
        private string databaseName;

        //Mocking Products To Test For This Unit Test
        List<Product> products = new List<Product>()
        {
            new Product{
                Id = 1,
                Title = "ProductIndex",
                Type = "Product",
                Category = "Vlees",
                ImageUrl = null,
                Description = "Product Description",
                Contents = "500g",
                DownloadLink = "product.pdf",
                ProductDetails = null },

            new Product{
                Id = 2,
                Title = "SecondProduct",
                Type = "Product",
                Category = "Zuivel",
                ImageUrl = null,
                Description = "Second Test Description",
                Contents = "1500g",
                DownloadLink = "secondproduct.pdf",
                ProductDetails = new List<ProductDetails>(){
                    new ProductDetails { Id = 1, Details = "Detail1", ProductId = 2 },
                    new ProductDetails { Id = 2, Details = "Detail2", ProductId = 2 }
                }
            }
        };

        //Gets The InMemory Database With The Mock Products
        private DatabaseContext GetInMemoryDbMetData()
        {
            var context = GetNewInMemoryDatabase(true);
            products.ForEach(p => context.Add(p));
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

        //Mocking a FormCollection Object
        private FormCollection GetFormCollection()
        {
            return new FormCollection(null);
        }

        //Mocking an IFormFile Object
        private IFormFile GetFormFile()
        {
            var path = Directory.GetCurrentDirectory();
            var url = path.Split("Test-Verstegen")[0];
            var webRoothPath = url + "Website-Verstegen\\" + "wwwroot\\";
            var fileMock = new Mock<IFormFile>();
            var fileName = webRoothPath + "images\\Products\\ontbijt.jpg";
            fileMock.Setup(_ => _.FileName).Returns(fileName);
            return fileMock.Object;
        }

        [Fact]
        public async Task IndexProductsController()
        {
            var context = GetInMemoryDbMetData();
            var controller = new ProductsController(context, _hostingEnvironment);

            var result = await controller.Index();

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<List<Product>>(viewResult.ViewData.Model);

            Assert.Equal(2, model.Count);
            Assert.Equal("ProductIndex", model[0].Title);
            Assert.NotNull(viewResult);
        }

        [Fact]
        public async Task ProductDetailsProductController()
        {
            var context = GetInMemoryDbMetData();
            var controller = new ProductsController(context, _hostingEnvironment);

            var result = await controller.ProductDetails(products[1].Id);
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<Product_ProductDetailViewModel>(viewResult.ViewData.Model);
            var product = model.Product;

            Assert.Equal(2, product.ProductDetails.Count);
            Assert.Equal(2, product.Id);
            Assert.Equal("Detail1", product.ProductDetails[0].Details);
            Assert.Equal(1, product.ProductDetails[0].Id);
        }

        [Fact]
        public async Task DetailsProductsController()
        {
            var context = GetInMemoryDbMetData();
            var controller = new ProductsController(context, _hostingEnvironment);

            var result = await controller.Details(products[0].Id);

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<Product>(viewResult.ViewData.Model);

            Assert.Equal(1, model.Id);
            Assert.Equal("ProductIndex", model.Title);
            Assert.NotNull(viewResult);
        }

        [Fact]
        public async Task PostCreateProductController()
        {
            //Getting Everything To Make This Unit Test Work
            FormCollection form = GetFormCollection();
            IFormFile formFile = GetFormFile();
            IHostingEnvironment hostingEnvironment = GetHostingEnvironment();
            DatabaseContext context = GetInMemoryDbMetData();

            var controller = new ProductsController(context, hostingEnvironment);

            var result = await controller.Create(products[0], formFile, form);
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", viewResult.ActionName);
        }

        [Fact]
        public async Task PostEditProductController()
        {
            //Getting Everything To Make This Unit Test Work
            FormCollection form = GetFormCollection();
            IFormFile formFile = GetFormFile();
            IHostingEnvironment hostingEnvironment = GetHostingEnvironment();
            DatabaseContext context = GetInMemoryDbMetData();

            var controller = new ProductsController(context, hostingEnvironment);

            var result = await controller.Edit(products[0].Id, products[0], formFile, form);
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", viewResult.ActionName);
        }

        [Fact]
        public async Task DeleteProductController()
        {
            DatabaseContext context = GetInMemoryDbMetData();

            var controller = new ProductsController(context, _hostingEnvironment);

            var result = await controller.DeleteConfirmed(products[1].Id);
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", viewResult.ActionName);
        }
    }
}