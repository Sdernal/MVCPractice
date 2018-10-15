using Microsoft.AspNetCore.Mvc;
using UnitTestApp.Controllers;
using Xunit;
using Moq;
using UnitTestApp.Models;
using System.Collections.Generic;
using System.Linq;

namespace UnitTestApp.Tests
{
    public class HomeControllerTests
    {
        [Fact]
        public void IndexViewDataMessage()
        {
            HomeController controller = new HomeController();
            ViewResult result = controller.Index() as ViewResult;
            Assert.Equal("Hello world!", result?.ViewData["Message"]);
        }

        [Fact]
        public void IndexViewResultNotNull()
        {
            HomeController controller = new HomeController();
            ViewResult result = controller.Index() as ViewResult;
            Assert.NotNull(result);
        }

        [Fact]
        public void IndexViewNameEqualIndex()
        {
            HomeController controller = new HomeController();
            ViewResult result = controller.Index() as ViewResult;
            Assert.Equal("Index", result?.ViewName);
        }

        [Fact]
        public void AboutReturnsAViewResultWithAListOfPhones()
        {
            var mock = new Mock<IRepository>();
            mock.Setup(repo => repo.GetAll()).Returns(GetTestPhones());
            var controller = new HomeController(mock.Object);

            var result = controller.About();

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Phone>>(viewResult.Model);
            Assert.Equal(GetTestPhones().Count, model.Count());
        }

        private List<Phone> GetTestPhones()
        {
            var phones = new List<Phone> {
                new Phone { Id=1, Name="IPhone Xs Max", Company="Apple", Price=130000 },
                new Phone {Id=2, Name="Meizu 16th", Company="Meizu", Price=45000},
                new Phone {Id=3, Name="Galaxy Note 9", Company="Samsung", Price=60000}
            };
            return phones;
        }

        [Fact]
        public void AddPhoneReturnsViewResultWithPhoneModel()
        {
            var mock = new Mock<IRepository>();
            var controller = new HomeController(mock.Object);
            controller.ModelState.AddModelError("Name", "Required");
            Phone newPhone = new Phone();

            var result = controller.AddPhone(newPhone);
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(newPhone, viewResult?.Model);
        }

        [Fact]
        public void AddPhoneReturnsARedirectAndAddsPhone()
        {
            var mock = new Mock<IRepository>();
            var controller = new HomeController(mock.Object);
            var newPhone = new Phone()
            {
                Name = "Sony Xperia XZ"
            };

            var result = controller.AddPhone(newPhone);

            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Null(redirectToActionResult.ControllerName);
            Assert.Equal("About", redirectToActionResult.ActionName);
            mock.Verify(r => r.Create(newPhone));
        }
    }
}
