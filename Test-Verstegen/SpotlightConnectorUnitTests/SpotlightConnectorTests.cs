using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Website_Verstegen.Controllers;
using Website_Verstegen.Data;
using Website_Verstegen.Models;
using Website_Verstegen.ViewModels;
using Xunit;

namespace Test_Verstegen.SpotlightConnectorUnitTests
{
    public class SpotlightConnectorTests
    {
        private string databaseName;

        //Mocking SpotlightConnector For this unit test
        List<SpotLightConnector> spotLightConnectors = new List<SpotLightConnector>()
        {
            new SpotLightConnector
            {
                Id = 1,
                 actionName = "Index",
                  controllerName = "Home",
                   SpotlightContent = new List<SpotlightContent>()
                   {
                       new SpotlightContent{ Id = 1, SpotlightConnectorId = 1, SpType = "Product", SpId = 5},
                       new SpotlightContent{ Id = 2, SpotlightConnectorId = 1, SpType = "Product", SpId = 6},
                       new SpotlightContent{ Id = 3, SpotlightConnectorId = 1, SpType = "Product", SpId = 7},
                       new SpotlightContent{ Id = 4, SpotlightConnectorId = 1, SpType = "Product", SpId = 8},
                       new SpotlightContent{ Id = 5, SpotlightConnectorId = 1, SpType = "Product", SpId = 9},
                       new SpotlightContent{ Id = 6, SpotlightConnectorId = 1, SpType = "Product", SpId = 10},
                       new SpotlightContent{ Id = 7, SpotlightConnectorId = 1, SpType = "Product", SpId = 11},
                       new SpotlightContent{ Id = 8, SpotlightConnectorId = 1, SpType = "Product", SpId = 12}
                   }
            },
            new SpotLightConnector
            {
                Id = 2,
                 actionName = "Create",
                  controllerName = "Products",
                   SpotlightContent = new List<SpotlightContent>()
                   {
                       new SpotlightContent{ Id = 9, SpotlightConnectorId = 2, SpType = "Product", SpId = 20},
                       new SpotlightContent{ Id = 10, SpotlightConnectorId = 2, SpType = "Packaging", SpId = 21},
                       new SpotlightContent{ Id = 11, SpotlightConnectorId = 2, SpType = "Product", SpId = 22},
                       new SpotlightContent{ Id = 12, SpotlightConnectorId = 2, SpType = "Packaging", SpId = 23},
                       new SpotlightContent{ Id = 13, SpotlightConnectorId = 2, SpType = "Packaging", SpId = 24},
                       new SpotlightContent{ Id = 14, SpotlightConnectorId = 2, SpType = "Product", SpId = 25},
                       new SpotlightContent{ Id = 15, SpotlightConnectorId = 2, SpType = "Packaging", SpId = 28},
                       new SpotlightContent{ Id = 16, SpotlightConnectorId = 2, SpType = "Product", SpId = 29}
                   }
            }
        };

        List<Product> products = new List<Product>()
        {
            new Product
            {
                Id = 1,
                Title = "ProductIndex",
                Type = "Product",
                Category = "Vlees",
                ImageUrl = null,
                Description = "Product Description",
                Contents = "500g",
                DownloadLink = "product.pdf",
                ProductDetails = null
            }
        };

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
            }
        };

        //Gets The InMemory Database With The Mock SpotlightConnectors
        private DatabaseContext GetInMemoryDbMetData()
        {
            var context = GetNewInMemoryDatabase(true);
            spotLightConnectors.ForEach(p => context.Add(p));
            products.ForEach(pr => context.Add(pr));
            packagings.ForEach(pa => context.Add(pa));
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

        [Fact]
        public async Task IndexSpotlightConnectorController()
        {
            var context = GetInMemoryDbMetData();
            var controller = new SpotLightConnectorsController(context);

            var result = await controller.Index();

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<List<SpotLightConnector>>(viewResult.ViewData.Model);

            Assert.Equal(2, model.Count);
            Assert.NotNull(viewResult);
            Assert.Equal("Index", model[0].actionName);
            Assert.Equal("Home", model[0].controllerName);

            Assert.Equal("Create", model[1].actionName);
            Assert.Equal("Products", model[1].controllerName);
        }

        [Fact]
        public void GetSpotlightViewModelSpotlightConnectorController()
        {
            var context = GetInMemoryDbMetData();
            var controller = new SpotLightConnectorsController(context);            

            var result = controller.GetSpotlightViewModel();

            var viewResult = Assert.IsType<SpotlightViewModel>(result);
            var model = Assert.IsAssignableFrom<SpotlightViewModel>(viewResult);
            
            //Tests if SpotLightConnector is 0, because this one is empty
            Assert.Equal(0, model.SpotLightConnector.Id);

            Assert.Equal(1, model.SpotLightItems[0].Id);
            Assert.Equal(2, model.SpotLightItems.Count);
        }

        [Fact]
        public void CreateSpotlightConnectorController()
        {
            var context = GetInMemoryDbMetData();
            var controller = new SpotLightConnectorsController(context);

            var result = controller.Create();

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<SpotlightViewModel>(viewResult.ViewData.Model);

            Assert.Equal(1, model.SpotLightItems[1].Id);
            Assert.Equal("ProductIndex", model.SpotLightItems[0].Title);
        }

        //Only possible with an illegal way
        //Can't Make this unit test because it is not possible to mock a formcollection object.
        [Fact]
        public async Task CreatePostSpotlightConnectorController()
        {
            var context = GetInMemoryDbMetData();
            var controller = new SpotLightConnectorsController(context);
          
            var result = await controller.Create(spotLightConnectors[0], null);
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", viewResult.ActionName);
        }

        [Fact]
        public async Task DeleteConfirmedSpotlightConnectorController()
        {
            var controller = new SpotLightConnectorsController(GetInMemoryDbMetData());
            var result = await controller.DeleteConfirmed(spotLightConnectors[1].Id);
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", viewResult.ActionName);
        }
    }
}
