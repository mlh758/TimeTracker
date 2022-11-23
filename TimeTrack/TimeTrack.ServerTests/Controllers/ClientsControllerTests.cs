using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TimeTrack.Server.Repositories;
using M = TimeTrack.Server.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using TimeTrack.Shared.ViewModels;
using TimeTrack.Shared;
using Microsoft.AspNetCore.Mvc;
using TimeTrack.Shared.Enums;

namespace TimeTrack.Server.Controllers.Tests
{
    [TestClass()]
    public class ClientsControllerTests
    {
        private static ClaimsPrincipal UserContext()
        {
            return new ClaimsPrincipal(new ClaimsIdentity(new Claim[] { new Claim(ClaimTypes.NameIdentifier, "user") }));
        }
        private static ClientsController Controller(IClientRepository r)
        {
            var controller = new ClientsController(r);
            controller.ControllerContext.HttpContext = new DefaultHttpContext() { User = UserContext() };
            return controller;
        }

        private static M.Client BuildClient(string abbreviation = "HELLO")
        {
            var category = new M.Category("CAT", CategoryType.Race) { Id = 123 };
            return new M.Client(abbreviation)
            {
                Disabilities = new List<M.Category>(),
                CustomDisabilities = new List<M.CustomCategory>(),
                Age = category,
                Race = category,
                Gender = category,
                Setting = category,
                SexualOrientation = category,
                Id = 123,
                UserId = "1"
            };
        }
        [TestMethod()]
        public void GetClientTest()
        {
            var mock = new Mock<IClientRepository>();

            mock.Setup(repo => repo.Find("user", 1)).ReturnsAsync(BuildClient());
            var controller = Controller(mock.Object);
            var task = controller.GetClient(1);
            task.Wait();
            Assert.IsNotNull(task.Result.Value);
            Assert.AreEqual(task.Result.Value.Abbreviation, "HELLO");
        }

        [TestMethod()]
        public void GetClientTest_WithCustomCategory()
        {
            var mock = new Mock<IClientRepository>();
            var client = BuildClient();
            client.CustomAge = new M.CustomCategory("DOG", CategoryType.Age, "user");
            mock.Setup(repo => repo.Find("user", 1)).ReturnsAsync(client);
            var controller = Controller(mock.Object);
            var task = controller.GetClient(1);
            task.Wait();
            Assert.AreEqual("DOG", task.Result.Value!.Age!.Name);
        }

        [TestMethod()]
        public void GetClientsTest()
        {
            var mock = new Mock<IClientRepository>();

            mock.Setup(repo => repo.All("user")).ReturnsAsync(new List<M.Client> { BuildClient("FIRST"), BuildClient("SECOND") });
            var controller = Controller(mock.Object);
            var task = controller.GetClients();
            task.Wait();
            Assert.AreEqual(task.Result.Count, 2);
        }

        private static ClientsController SetupCreate(NewClientForm with)
        {
            var mock = new Mock<IClientRepository>();
            mock.Setup(repo => repo.Create("user", with)).ReturnsAsync(BuildClient("FIRST"));
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
            var category = new Category("CAT") { Id = 123, Type = CategoryType.Race };
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