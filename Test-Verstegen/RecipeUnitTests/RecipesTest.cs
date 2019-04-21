using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Website_Verstegen.Controllers;
using Website_Verstegen.Data;
using Website_Verstegen.Models;
using Xunit;
using System.IO;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Threading.Tasks;

namespace Test_Verstegen.RecipeUnitTests
{
    public class RecipesTest
    {
        private readonly DatabaseContext _context;
        private IHostingEnvironment _hostingEnvironment;

        private string databaseName;

        private List<Ingredient> Ingredients = new List<Ingredient>()
        {
            new Ingredient
            {
                Id = 1,
                Name = "Sugar",
                Amount = 100,
                Unit = "Grams"
            },
            new Ingredient
            {
                Id = 2,
                Name = "Apples",
                Amount = 1,
                Unit = "Piece"
            },
            new Ingredient
            {
                Id = 3,
                Name = "Meat",
                Amount = 300,
                Unit = "Grams"
            },
            new Ingredient
            {
                Id = 4,
                Name = "Spider",
                Amount = 50,
                Unit = "Pieces"
            },
        };

        private List<Recipe> recipes = new List<Recipe>()
        {
            new Recipe
            {
                Id = 1,
                ImageUrl = "/images/recipes/apple-pie-1071747_640.jpg",
                Category = "Bakery",
                PreparationSteps = new List<PreparationStep>()
                {
                    new PreparationStep
                    {
                        PreparationStepId = 1,
                        RecipeId = 1,
                        Text = "slice the apples"

                    },
                    new PreparationStep
                    {
                        PreparationStepId = 2,
                        RecipeId = 1,
                        Text = "Heat up the suger to make flames"
                    }
                },
                Title = "Apple pie surprise",
                RecipeIngredients = new List<RecipeIngredient>()
                {
                    new RecipeIngredient
                    {
                        RecipeIngredientId = 1,
                        IngredientId = 1,
                        RecipeId =  1
                    },
                    new RecipeIngredient
                    {
                        RecipeIngredientId = 2,
                        IngredientId = 2,
                        RecipeId = 1
                    }
                }
            },
            new Recipe
            {
                Id = 2,
                ImageUrl = "/images/recipes/pizza-346985_640.jpg",
                Category = "Bakery",
                PreparationSteps = new List<PreparationStep>()
                {
                    new PreparationStep
                    {
                        PreparationStepId = 3,
                        RecipeId = 2,
                        Text = "Make dough"

                    },
                    new PreparationStep
                    {
                        PreparationStepId = 4,
                        RecipeId = 2,
                        Text = "Heat up the dough"
                    }
                },
                Title = "Vleesspider",
                RecipeIngredients = new List<RecipeIngredient>()
                {
                    new RecipeIngredient
                    {
                        RecipeIngredientId = 3,
                        IngredientId = 3,
                        RecipeId =  2
                    },
                    new RecipeIngredient
                    {
                        RecipeIngredientId = 4,
                        IngredientId = 4,
                        RecipeId = 2
                    }
                }
            }
        };

        //Gets The InMemory Database With The Mock Products
        private DatabaseContext GetInMemoryDbMetData()
        {
            var context = GetNewInMemoryDatabase(true);
            recipes.ForEach(p => context.Add(p));
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
        public async Task IndexRecipesController()
        {
            var context = GetInMemoryDbMetData();
            var controller = new RecipesController(context, _hostingEnvironment);

            var result = await controller.Index();

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<List<Recipe>>(viewResult.ViewData.Model);

            Assert.Equal(2, model.Count);
            Assert.Equal("Bakery", model[0].Category);
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

            var controller = new RecipesController(context, hostingEnvironment);

            var result = await controller.Create(recipes[0], formFile, form);
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

            var controller = new RecipesController(context, hostingEnvironment);

            var result = await controller.Edit(recipes[0].Id, recipes[0], formFile, form);
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", viewResult.ActionName);
        }

        [Fact]
        public async Task DeleteProductController()
        {
            DatabaseContext context = GetInMemoryDbMetData();

            var controller = new RecipesController(context, _hostingEnvironment);

            var result = await controller.DeleteConfirmed(recipes[1].Id);
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", viewResult.ActionName);
        }


    }
}
