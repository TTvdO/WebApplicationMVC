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

namespace Test_Verstegen.CertificateUnitTests
{
    public class CertificateTests
    {
        private string databaseName;

        //Mocking Certificates To Test For This Unit Test
        List<Certificate> certificates = new List<Certificate>()
        {
            new Certificate{
                 Id = 1,
                  Name = "Certificate 1",
                   Alt_Text = "First Cert",
                    Img_Src = null
            },
            new Certificate{
                 Id = 2,
                  Name = "Certificate 2",
                   Alt_Text = "Second Cert",
                    Img_Src = null
            },
            new Certificate{
                 Id = 3,
                  Name = "Certificate 3",
                   Alt_Text = "Third Cert",
                    Img_Src = null
            },
        };

        //Gets The InMemory Database With The Mock Certificates
        private DatabaseContext GetInMemoryDbMetData()
        {
            var context = GetNewInMemoryDatabase(true);
            certificates.ForEach(p => context.Add(p));
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
            var fileName = webRoothPath + "images\\Certificates\\ontbijt.jpg";
            fileMock.Setup(_ => _.FileName).Returns(fileName);
            return fileMock.Object;
        }

        [Fact]
        public async Task IndexCertificatesController()
        {
            var controller = new CertificatesController(GetInMemoryDbMetData(), GetHostingEnvironment());

            var result = await controller.Index();
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<List<Certificate>>(viewResult.ViewData.Model);
            
            Assert.Equal("Certificate 1", model[0].Name);
            Assert.Equal("Certificate 2", model[1].Name);
            Assert.Equal("Certificate 3", model[2].Name);

            Assert.Equal("First Cert", model[0].Alt_Text);
            Assert.Equal("Second Cert", model[1].Alt_Text);
            Assert.Equal("Third Cert", model[2].Alt_Text);

            Assert.Equal(3, model.Count);
            Assert.NotNull(viewResult);
        }

        [Fact]
        public async Task DetailsCertificatesController()
        {
            var controller = new CertificatesController(GetInMemoryDbMetData(), GetHostingEnvironment());

            var result = await controller.Details(certificates[1].Id);

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<Certificate>(viewResult.ViewData.Model);

            Assert.Equal(2, model.Id);
            Assert.Equal("Certificate 2", model.Name);
            Assert.Equal("Second Cert", model.Alt_Text);
            Assert.NotNull(viewResult);
        }

        [Fact]
        public async Task CreateCertificatesController()
        {
            var controller = new CertificatesController(GetInMemoryDbMetData(), GetHostingEnvironment());

            var result = await controller.Create(certificates[2], GetFormFile());
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", viewResult.ActionName);
        }

        [Fact]
        public async Task EditCertificatesController()
        {
            var controller = new CertificatesController(GetInMemoryDbMetData(), GetHostingEnvironment());

            var result = await controller.Edit(certificates[0].Id, certificates[0], GetFormFile());
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", viewResult.ActionName);
        }

        [Fact]
        public async Task DeleteConfirmedCertificatesController()
        {
            var controller = new CertificatesController(GetInMemoryDbMetData(), GetHostingEnvironment());

            var result = await controller.DeleteConfirmed(certificates[1].Id);
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", viewResult.ActionName);
        }
    }
}
