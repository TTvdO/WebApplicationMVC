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

namespace Test_Verstegen.ThemesUnitTests
{
    public class ThemesUnitTest
    {
        private string DatabaseName;

        [Fact]
        public async Task IndexTest()
        {
            //HostingEnvironment to pass into the ThemesController constructor
            IHostingEnvironment hostingEnvironment = GetHostingEnvironment();

            //Get In Memory Database
            DatabaseContext context = GetNewInMemoryDbWithData();

            //Mock the ThemesController with the In Memory Database as a context
            var controller = new ThemesController(context, hostingEnvironment);

            //Call the Index method
            var result = await controller.TestIndex();
            //Check if the Index method returns a View
            var viewResult = Assert.IsType<ViewResult>(result);
            //Check if the Model passed to the View in the return statement can be made into a List<Theme>. IEnumerable<Theme> works. Return class has to be of type Theme ofcourse
            var model = Assert.IsAssignableFrom<List<Theme>>(viewResult.ViewData.Model);
            
            //Make sure that the passed model has only three instances in it, just as initialized in the In Memory Database
            Assert.Equal(3, model.Count);

            //Check if every Theme has the correct amount of ThemeProducts. 2, 1, 0
            Assert.Equal(2, model[0].ThemeProducts.Count);
            Assert.Single(model[1].ThemeProducts);
            Assert.Empty(model[2].ThemeProducts);

            //Check if all the id's are correct
            Assert.Equal(1, model[0].ThemeId);
            Assert.Equal(2, model[1].ThemeId);
            Assert.Equal(3, model[2].ThemeId);

            //Check if all the ThemeNames are correct
            Assert.Equal("Winter", model[0].ThemeName);
            Assert.Equal("Zomer", model[1].ThemeName);
            Assert.Equal("Herfst", model[2].ThemeName);

            //Check if all the Theme description's are correct
            Assert.Equal("Zeer koud", model[0].Description);
            Assert.Equal("Zeer warm", model[1].Description);
            Assert.Equal("Tussen warm en koud in", model[2].Description);

            //Check if all the images are correct
            Assert.Equal("\\images\\themes\\farmersausages.jpg", model[0].ImageUrl);
            Assert.Equal("\\images\\themes\\piment.jpg", model[1].ImageUrl);
            Assert.Equal("\\images\\themes\\turkey.jpg", model[2].ImageUrl);

            //Check if all the Themes have the correct relationship with a Product. Get to the Product by going to ThemeProducts first. Check if all the attributes of the Product are
            //correctly initialized
            Assert.Equal(1, model[0].ThemeProducts[0].Product.Id);
            Assert.Equal(2, model[0].ThemeProducts[1].Product.Id);
            Assert.Equal("Test product1", model[0].ThemeProducts[0].Product.Title);
            Assert.Equal("Product naam twee", model[0].ThemeProducts[1].Product.Title);
            Assert.Equal("Testen", model[0].ThemeProducts[0].Product.Category);
            Assert.Equal("Product namen", model[0].ThemeProducts[1].Product.Category);
            Assert.Equal("Goede beschrijving", model[0].ThemeProducts[0].Product.Contents);
            Assert.Equal("Een product", model[0].ThemeProducts[1].Product.Contents);
            Assert.Equal("\\images\\products\\food-1.PNG", model[0].ThemeProducts[0].Product.ImageUrl);
            Assert.Equal("\\images\\products\\TW_ext.jpg", model[0].ThemeProducts[1].Product.ImageUrl);
            Assert.Equal("Een goede omschrijving", model[0].ThemeProducts[0].Product.Description);
            Assert.Equal("Een product met een naam", model[0].ThemeProducts[1].Product.Description);
            Assert.Equal("pagina.pdf", model[0].ThemeProducts[0].Product.DownloadLink);
            Assert.Equal("GoogleMapsAPI.com", model[0].ThemeProducts[1].Product.DownloadLink);
        }

        [Fact]
        public async Task DetailsTest()
        {
            DatabaseContext context = GetNewInMemoryDbWithData();

            IHostingEnvironment hostingEnvironment = GetHostingEnvironment();

            var controller = new ThemesController(context, hostingEnvironment);

            var result = await controller.Details(2);
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<Theme>(viewResult.ViewData.Model);

            //Make sure that Theme with id number 2 has the correct details
            Assert.Equal(2, model.ThemeId);
            Assert.Equal("Zomer", model.ThemeName);
            Assert.Equal("Zeer warm", model.Description);
            Assert.Equal("\\images\\themes\\piment.jpg", model.ImageUrl);
        }

        [Fact]
        public async Task CreateTest()
        {
            IHostingEnvironment hostingEnvironment = GetHostingEnvironment();

            DatabaseContext context = GetNewInMemoryDbWithData();

            var controller = new ThemesController(context, hostingEnvironment);

            //Make a new theme to add
            Theme NewTheme = new Theme()
            {
                ThemeId = 4,
                ThemeName = "Nieuwe thema naam",
                Description = "Nieuwe omschrijving",
                ImageUrl = "images\\themes\\piment.jpg"
            };

            //Make a new FormFile using the ImageUrl
            IFormFile FormFile = GetFormFile(NewTheme.ImageUrl);

            //Create the NewTheme with the given FormFile
            var result = await controller.Create(NewTheme, FormFile);

            //Check if all values are correct
            Assert.Equal(4, context.Themes.Find(4).ThemeId);
            Assert.Equal("Nieuwe thema naam", context.Themes.Find(4).ThemeName);
            Assert.Equal("Nieuwe omschrijving", context.Themes.Find(4).Description);
            Assert.Equal("\\images\\themes\\piment.jpg", context.Themes.Find(4).ImageUrl);

            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Null(viewResult.ControllerName);
            Assert.Equal("Index", viewResult.ActionName);
        }

        [Fact]
        public async Task EditTest()
        {
            IHostingEnvironment hostingEnvironment = GetHostingEnvironment();

            DatabaseContext context = GetNewInMemoryDbWithData();

            var controller = new ThemesController(context, hostingEnvironment);

            //Get a Theme with Id 1 from the In Memory Database
            Theme ThemeToEdit = context.Themes.Find(1);

            //Change the values
            ThemeToEdit.ThemeName = "Veranderd";
            ThemeToEdit.ImageUrl = "images\\themes\\farmersausages.jpg";
            ThemeToEdit.Description = "Nieuwe omschrijving";

            //Change the FormFile to the current ImageUrl
            IFormFile FormFile = GetFormFile(ThemeToEdit.ImageUrl);

            //Edit the Theme
            var result = await controller.Edit(1, ThemeToEdit, FormFile);

            //Check if edited values are the same as above
            Assert.Equal("Veranderd", context.Themes.Find(1).ThemeName);
            Assert.Equal("\\images\\themes\\farmersausages.jpg", context.Themes.Find(1).ImageUrl);
            Assert.Equal("Nieuwe omschrijving", context.Themes.Find(1).Description);

            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Null(viewResult.ControllerName);
            Assert.Equal("Index", viewResult.ActionName);
        }

        [Fact]
        public async Task DeleteConfirmedTest()
        {
            DatabaseContext context = GetNewInMemoryDbWithData();

            IHostingEnvironment hostingEnvironment = GetHostingEnvironment();

            var controller = new ThemesController(context, hostingEnvironment);

            //Get the first theme from the In Memory Database
            Theme ThemeBeforeDelete = context.Themes.FirstOrDefault(t => t.ThemeName != null);
            //Make sure all the values are correct
            Assert.Equal("Winter", ThemeBeforeDelete.ThemeName);
            Assert.Equal("Zeer koud", ThemeBeforeDelete.Description);
            Assert.Equal("\\images\\themes\\farmersausages.jpg", ThemeBeforeDelete.ImageUrl);

            //Delete the first theme
            var result = await controller.DeleteConfirmed(ThemeBeforeDelete.ThemeId);

            //Get the first theme from the In Memory Database, after deletion
            Theme ThemeAfterDelete = context.Themes.FirstOrDefault(t => t.ThemeName != null);
            //Make sure all the values are correct
            Assert.Equal("Zomer", ThemeAfterDelete.ThemeName);
            Assert.Equal("Zeer warm", ThemeAfterDelete.Description);
            Assert.Equal("\\images\\themes\\piment.jpg", ThemeAfterDelete.ImageUrl);

            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Null(viewResult.ControllerName);
            Assert.Equal("Index", viewResult.ActionName);
        }

        private DatabaseContext GetNewInMemoryDbWithData()
        {
            //Get new in memory db with new generated name
            DatabaseContext context = GetNewInMemoryDb(true);

            //Initialize themes with ThemeProduct and Product relationships
            List<Theme> Themes = new List<Theme>
            {
                new Theme
                {
                    ThemeId = 1,
                    ThemeName = "Winter",
                    Description = "Zeer koud",
                    ImageUrl = "\\images\\themes\\farmersausages.jpg",
                    ThemeProducts = new List<ThemeProduct>()
                    {
                        new ThemeProduct
                        {
                            ThemeProductId = 1,
                            Product = new Product
                            {
                                Id = 1,
                                Title = "Test product1",
                                Category = "Testen",
                                Contents = "Goede beschrijving",
                                ImageUrl = "\\images\\products\\food-1.PNG",
                                Description = "Een goede omschrijving",
                                DownloadLink = "pagina.pdf",
                            }
                        },
                        new ThemeProduct
                        {
                            ThemeProductId = 2,
                            Product = new Product
                            {
                                Id = 2,
                                Title = "Product naam twee",
                                Category = "Product namen",
                                Contents = "Een product",
                                ImageUrl = "\\images\\products\\TW_ext.jpg",
                                Description = "Een product met een naam",
                                DownloadLink = "GoogleMapsAPI.com"
                            }
                        }
                    }
                },
                new Theme
                {
                    ThemeId = 2,
                    ThemeName = "Zomer",
                    Description = "Zeer warm",
                    ImageUrl = "\\images\\themes\\piment.jpg",
                    ThemeProducts = new List<ThemeProduct>()
                    {
                        new ThemeProduct
                        {
                            ThemeProductId = 3,
                            Product = new Product
                            {
                                Id = 3,
                                Title = "Titel voorbeeld",
                                Category = "Voorbeelden",
                                Contents = "Voorbeeld content",
                                ImageUrl = "\\images\\products\\ontbijt.jpg",
                                Description = "Voorbeeld beschrijving",
                                DownloadLink = "Voorbeelden.gov",
                            }
                        }
                    }
                },
                new Theme
                {
                    ThemeId = 3,
                    ThemeName = "Herfst",
                    Description = "Tussen warm en koud in",
                    ImageUrl = "\\images\\themes\\turkey.jpg",
                }
            };

            //Add all the Themes from the list above to the In Memory Database
            context.Themes.AddRange(Themes);

            context.SaveChanges();

            //Return the database, don't generate a new name
            return GetNewInMemoryDb(false);
        }

        private DatabaseContext GetNewInMemoryDb(bool NewDb)
        {
            //Generate new name
            if (NewDb) this.DatabaseName = Guid.NewGuid().ToString();

            var options = new DbContextOptionsBuilder<DatabaseContext>().UseInMemoryDatabase(this.DatabaseName).Options;

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
        private IFormFile GetFormFile(string imagePath)
        {
            var path = Directory.GetCurrentDirectory();
            var url = path.Split("Test-Verstegen")[0];
            var webRoothPath = url + "Website-Verstegen\\" + "wwwroot\\";
            var fileMock = new Mock<IFormFile>();
            var fileName = webRoothPath + imagePath;
            fileMock.Setup(f => f.FileName).Returns(fileName);
            return fileMock.Object;
        }
    }
}
