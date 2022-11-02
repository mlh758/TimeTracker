using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TimeTrack.Server.Repositories;
using M = TimeTrack.Shared.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using TimeTrack.Shared.ViewModels;
using TimeTrack.Shared;
using Microsoft.AspNetCore.Mvc;

namespace TimeTrack.Server.Controllers.Tests
{
    [TestClass()]
    public class ClientsControllerTests
    {
        private static ClaimsPrincipal UserContext()
        {
            return new ClaimsPrincipal(new ClaimsIdentity(new Claim[] { new Claim("UserID", "1") }));
        }
        private static ClientsController Controller(IClientRepository r)
        {
            var controller = new ClientsController(r);
            controller.ControllerContext.HttpContext = new DefaultHttpContext() { User = UserContext() };
            return controller;
        }

        private static M.Client BuildClient(string abbreviation = "HELLO")
        {
            var category = new M.Category("CAT", M.CategoryType.Race) { Id = 123 };
            return new M.Client(abbreviation)
            {
                Disabilities = new List<M.Category>(),
                Age = category,
                Race = category,
                Gender = category,
                Setting = category,
                SexualOrientation = category,
                Id = 123,
            };
        }
        [TestMethod()]
        public void GetClientTest()
        {
            var mock = new Mock<IClientRepository>();

            mock.Setup(repo => repo.Find(1, 1)).ReturnsAsync(BuildClient());
            var controller = Controller(mock.Object);
            var task = controller.GetClient(1);
            task.Wait();
            Assert.AreEqual(task.Result.Value.Abbreviation, "HELLO");
        }

        [TestMethod()]
        public void GetClientsTest()
        {
            var mock = new Mock<IClientRepository>();

            mock.Setup(repo => repo.All(1)).ReturnsAsync(new List<M.Client> { BuildClient("FIRST"), BuildClient("SECOND") });
            var controller = Controller(mock.Object);
            var task = controller.GetClients();
            task.Wait();
            Assert.AreEqual(task.Result.Count, 2);
        }

        private static ClientsController SetupCreate(NewClientForm with)
        {
            var mock = new Mock<IClientRepository>();
            mock.Setup(repo => repo.Create(1, with)).ReturnsAsync(BuildClient("FIRST"));
            return Controller(mock.Object);
        }

        [TestMethod()]
        public void CreateClientTest_IncompleteData()
        {
            var form = new NewClientForm();
            var controller = SetupCreate(form);
            controller.ModelState.AddModelError("Whatever", "Required");
            var task = controller.CreateClient(form);
            task.Wait();
            Assert.IsInstanceOfType(task.Result.Result, typeof(BadRequestObjectResult));
        }

        [TestMethod()]
        public void CreateClientTest_ValidData()
        {
            var category = new Category("CAT") { Id = 123, Type = M.CategoryType.Race };
            var form = new NewClientForm()
            {
                Abbreviation = "HI",
                Age = category,
                Setting = category,
                SexualOrientation = category,
                Gender = category,
                Race = category,
            };
            var controller = SetupCreate(form);
            var task = controller.CreateClient(form);
            task.Wait();
            Assert.IsInstanceOfType(task.Result.Result, typeof(CreatedAtActionResult));
        }

        [TestMethod()]
        public void DeleteClientTest()
        {
            var mock = new Mock<IClientRepository>();
            var controller = Controller(mock.Object);
            var task = controller.DeleteClient(1);
            task.Wait();
            Assert.IsInstanceOfType(task.Result, typeof(OkResult));

        }
    }
}