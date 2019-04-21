using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Website_Verstegen.Controllers;
using Website_Verstegen.Data;
using Website_Verstegen.Models;
using Xunit;

namespace Test_Verstegen.ThemeProductsUnitTests
{
    public class ThemesProductsUnitTest
    {
        private string DatabaseName;

        [Fact]
        public async Task IndexTest()
        {
            //Get In Memory Database
            DatabaseContext context = GetNewInMemoryDbWithData();

            //Mock the ThemeProductsController with the In Memory Database as a context
            var controller = new ThemeProductsController(context);

            //Call the Index method
            var result = await controller.Index();
            //Check if the Index method returns a View
            var viewResult = Assert.IsType<ViewResult>(result);
            //Check if the Model passed to the View in the return statement can be made into a List<ThemeProduct>. IEnumerable<ThemeProduct> works. Return class has to be of type ThemeProduct ofcourse
            var model = Assert.IsAssignableFrom<List<ThemeProduct>>(viewResult.ViewData.Model);

            //Make sure that the passed model has only three instances in it, just as initialized in the In Memory Database
            Assert.Equal(3, model.Count);

            //Check if every ThemeProduct has the correct ThemeId and Id(from Product)
            Assert.Equal(4, model[0].ThemeId);
            Assert.Equal(5, model[1].ThemeId);
            Assert.Equal(6, model[2].ThemeId);
            Assert.Equal(4, model[0].Id);
            Assert.Equal(5, model[1].Id);
            Assert.Equal(6, model[2].Id);

            //Check if every ThemeProduct has the correct ThemeName and Product Title
            Assert.Equal("Waterpolo", model[0].Theme.ThemeName);
            Assert.Equal("Basketbal", model[1].Theme.ThemeName);
            Assert.Equal("Voetbal", model[2].Theme.ThemeName);
            Assert.Equal("Test product4", model[0].Product.Title);
            Assert.Equal("Product nummer vijf", model[1].Product.Title);
            Assert.Equal("Product titel", model[2].Product.Title);

            //Check if every Theme and Product description match
            Assert.Equal("Nat", model[0].Theme.Description);
            Assert.Equal("Gooien", model[1].Theme.Description);
            Assert.Equal("Veld", model[2].Theme.Description);
            Assert.Equal("Mooie omschrijving", model[0].Product.Description);
            Assert.Equal("Zeer groot", model[1].Product.Description);
            Assert.Equal("Product omschrijving", model[2].Product.Description);

            //Check if every Theme and Product have the correct ImageUrl
            Assert.Equal("\\images\\themes\\turkey.jpg", model[0].Theme.ImageUrl);
            Assert.Equal("\\images\\themes\\beakers.jpg", model[1].Theme.ImageUrl);
            Assert.Equal("\\images\\themes\\cutterbags.jpg", model[2].Theme.ImageUrl);
            Assert.Equal("\\images\\products\\fish.jpg", model[0].Product.ImageUrl);
            Assert.Equal("\\images\\products\\vi.png", model[1].Product.ImageUrl);
            Assert.Equal("\\images\\products\\groentes.png", model[2].Product.ImageUrl);

            //Check if every Product has the correct Category
            Assert.Equal("Testing", model[0].Product.Category);
            Assert.Equal("Grote test", model[1].Product.Category);
            Assert.Equal("Product categorie", model[2].Product.Category);

            //Check if every Product has the correct Contents
            Assert.Equal("Solid beschrijving", model[0].Product.Contents);
            Assert.Equal("Groot", model[1].Product.Contents);
            Assert.Equal("Product inhoud", model[2].Product.Contents);

            //Check if every Product has the correct DownloadLink
            Assert.Equal("product.pdf", model[0].Product.DownloadLink);
            Assert.Equal("groot.pdf", model[1].Product.DownloadLink);
            Assert.Equal("Product.gov", model[2].Product.DownloadLink);
        }

        [Fact]
        public async Task DetailsTest()
        {
            DatabaseContext context = GetNewInMemoryDbWithData();

            var controller = new ThemeProductsController(context);

            var result = await controller.Details(4);
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<ThemeProduct>(viewResult.ViewData.Model);

            //Check if the Theme with id 4 has the correct values
            Assert.Equal(4, model.Theme.ThemeId);
            Assert.Equal("Waterpolo", model.Theme.ThemeName);
            Assert.Equal("Nat", model.Theme.Description);
            Assert.Equal("\\images\\themes\\turkey.jpg", model.Theme.ImageUrl);

            //Check if the Product with id 4 has the correct values
            Assert.Equal(4, model.Product.Id);
            Assert.Equal("Test product4", model.Product.Title);
            Assert.Equal("Mooie omschrijving", model.Product.Description);
            Assert.Equal("\\images\\products\\fish.jpg", model.Product.ImageUrl);
        }

        [Fact]
        public async Task CreateTest()
        {
            DatabaseContext context = GetNewInMemoryDbWithData();

            var controller = new ThemeProductsController(context);

            //Create a new ThemeProduct to add
            ThemeProduct NewThemeProduct = new ThemeProduct()
            {
                ThemeProductId = 7,
                Product = new Product
                {
                    Id = 7,
                    Title = "Nieuw Product",
                    Category = "Create",
                    Contents = "Nieuw Product beschrijving",
                    ImageUrl = "\\images\\products\\fishxd.jpg",
                    Description = "Product create omschrijving",
                    DownloadLink = "productCreation.pdf",
                },
                Theme = new Theme
                {
                    ThemeId = 7,
                    ThemeName = "Nieuw Thema",
                    Description = "Thema create omschrijving",
                    ImageUrl = "\\images\\themes\\beakers.jpg"
                }
            };

            //Create the new ThemeProduct
            var result = await controller.Create(NewThemeProduct);

            //Make sure it has the correct ThemeProductId
            Assert.Equal(7, context.ThemeProducts.Find(7).ThemeProductId);

            //Make sure the Product has all the correct values
            Assert.Equal(7, context.ThemeProducts.Find(7).Product.Id);
            Assert.Equal("Nieuw Product", context.ThemeProducts.Find(7).Product.Title);
            Assert.Equal("Create", context.ThemeProducts.Find(7).Product.Category);
            Assert.Equal("Nieuw Product beschrijving", context.ThemeProducts.Find(7).Product.Contents);
            Assert.Equal("\\images\\products\\fishxd.jpg", context.ThemeProducts.Find(7).Product.ImageUrl);
            Assert.Equal("Product create omschrijving", context.ThemeProducts.Find(7).Product.Description);
            Assert.Equal("productCreation.pdf", context.ThemeProducts.Find(7).Product.DownloadLink);

            //Make sure the Theme has all the correct values
            Assert.Equal(7, context.ThemeProducts.Find(7).Theme.ThemeId);
            Assert.Equal("Nieuw Thema", context.ThemeProducts.Find(7).Theme.ThemeName);
            Assert.Equal("Thema create omschrijving", context.ThemeProducts.Find(7).Theme.Description);
            Assert.Equal("\\images\\themes\\beakers.jpg", context.ThemeProducts.Find(7).Theme.ImageUrl);

            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Null(viewResult.ControllerName);
            Assert.Equal("Index", viewResult.ActionName);
        }

        [Fact]
        public async Task DeleteConfirmedTest()
        {
            DatabaseContext context = GetNewInMemoryDbWithData();

            var controller = new ThemeProductsController(context);

            //Get the first ThemeProduct from the In Memory Database
            ThemeProduct ThemeProductBeforeDelete = context.ThemeProducts.FirstOrDefault(t => t.Theme != null && t.Product != null);
            //Make sure it has the correct values
            Assert.Equal(4, ThemeProductBeforeDelete.ThemeProductId);
            Assert.Equal(4, ThemeProductBeforeDelete.ThemeId);
            Assert.Equal(4, ThemeProductBeforeDelete.Id);

            //Delete the first ThemeProduct
            var result = await controller.DeleteConfirmed(ThemeProductBeforeDelete.ThemeId);

            //Get the first ThemeProduct from the In Memory Database after deletion
            ThemeProduct ThemeProductAfterDelete = context.ThemeProducts.FirstOrDefault(t => t.Theme != null && t.Product != null);
            Assert.Equal(5, ThemeProductAfterDelete.ThemeProductId);
            Assert.Equal(5, ThemeProductAfterDelete.ThemeId);
            Assert.Equal(5, ThemeProductAfterDelete.Id);

            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Null(viewResult.ControllerName);
            Assert.Equal("Index", viewResult.ActionName);
        }

        private DatabaseContext GetNewInMemoryDbWithData()
        {
            //Get a new In Memory Database with a newly generated name
            DatabaseContext context = GetNewInMemoryDb(true);

            //Initialize a ThemeProducts list for the In Memory Database
            List<ThemeProduct> ThemeProducts = new List<ThemeProduct>
            {
                new ThemeProduct
                {
                    ThemeProductId = 4,
                    Product = new Product
                    {
                        Id = 4,
                        Title = "Test product4",
                        Category = "Testing",
                        Contents = "Solid beschrijving",
                        ImageUrl = "\\images\\products\\fish.jpg",
                        Description = "Mooie omschrijving",
                        DownloadLink = "product.pdf",
                    },
                    Theme = new Theme
                    {
                        ThemeId = 4,
                        ThemeName = "Waterpolo",
                        Description = "Nat",
                        ImageUrl = "\\images\\themes\\turkey.jpg"
                    },
                },
                new ThemeProduct
                {
                    ThemeProductId = 5,
                    Product = new Product
                    {
                        Id = 5,
                        Title = "Product nummer vijf",
                        Category = "Grote test",
                        Contents = "Groot",
                        ImageUrl = "\\images\\products\\vi.png",
                        Description = "Zeer groot",
                        DownloadLink = "groot.pdf",
                    },
                    Theme = new Theme
                    {
                        ThemeId = 5,
                        ThemeName = "Basketbal",
                        Description = "Gooien",
                        ImageUrl = "\\images\\themes\\beakers.jpg"
                    }
                },
                new ThemeProduct
                {
                    ThemeProductId = 6,
                    Product = new Product
                    {
                        Id = 6,
                        Title = "Product titel",
                        Category = "Product categorie",
                        Contents = "Product inhoud",
                        ImageUrl = "\\images\\products\\groentes.png",
                        Description = "Product omschrijving",
                        DownloadLink = "Product.gov",
                    },
                    Theme = new Theme
                    {
                        ThemeId = 6,
                        ThemeName = "Voetbal",
                        Description = "Veld",
                        ImageUrl = "\\images\\themes\\cutterbags.jpg"
                    }
                }
            };

            //Add the list above to the In Memory Database
            context.ThemeProducts.AddRange(ThemeProducts);

            context.SaveChanges();

            //Return the current In Memory Database, don't generate a new name
            return GetNewInMemoryDb(false);
        }

        private DatabaseContext GetNewInMemoryDb(bool NewDb)
        {
            //Generate new name
            if (NewDb) this.DatabaseName = Guid.NewGuid().ToString();

            var options = new DbContextOptionsBuilder<DatabaseContext>().UseInMemoryDatabase(this.DatabaseName).Options;

            return new DatabaseContext(options);
        }
    }
}