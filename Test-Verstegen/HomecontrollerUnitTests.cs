using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Website_Verstegen.Controllers;
using Website_Verstegen.Data;
using Website_Verstegen.Models;
using Xunit;

namespace Test_Verstegen
{
    public class HomecontrollerUnitTests
    {
        private readonly DatabaseContext context;
        private string databaseName;

        private DatabaseContext GetInMemoryDbMetData()
        {
            var context = GetNewInMemoryDatabase(true);
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
        public void IndexTest()
        {
            var context = GetInMemoryDbMetData();
            var controller = new HomeController(context);

            var result =  controller.Index();
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.NotNull(viewResult);
        }
    }
}
