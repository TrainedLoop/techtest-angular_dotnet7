using Builders.Bills.API.Controllers;
using Builders.Bills.API.Models;
using Builders.Bills.Services.Interfaces;
using Builders.Bills.Shared;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Builders.Tests.Bills.API
{
    [TestClass]
    public class BillControllerTests
    {
        private Mock<IBillCalculatorService> mockBillCalculatorService;
        private BillController billController;

        [TestInitialize]
        public void Setup()
        {
            mockBillCalculatorService = new Mock<IBillCalculatorService>();
            billController = new BillController(mockBillCalculatorService.Object);
        }

        [TestMethod]
        public void Post_ShouldReturnNotFound_WhenBillIsNotFound()
        {
            mockBillCalculatorService.Setup(x => x.GetCalculatedBill(It.IsAny<string>(), It.IsAny<DateTime>()))
                .Returns(Task.FromResult<IBillCalculated?>(null));

            var requestBody = new CalculateFeeRequest("12345678901234567890123456789012345678901234", "2022-04-14");

            var result = billController.Post(requestBody).Result;

            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void Post_ShouldReturnOk_WhenBillIsFound()
        {
            mockBillCalculatorService.Setup(x => x.GetCalculatedBill(It.IsAny<string>(), It.IsAny<DateTime>()))
                .ReturnsAsync(Mock.Of<IBillCalculated>());

            var requestBody = new CalculateFeeRequest("12345678901234567890123456789012345678901234", "2022-04-14");

            var result = billController.Post(requestBody).Result;

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }
    }
}