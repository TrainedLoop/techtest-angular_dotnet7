using Builders.Bills.API.Models;
using Microsoft.AspNetCore.Http;

namespace Builders.Tests.Bills.API.Models
{
    [TestClass]
    public class CalculateFeeRequestTests
    {
        [TestMethod]
        public void CalculateFeeRequest_InitializedWithValidData_PropertiesAreSetCorrectly()
        {
            string barcode = "34191790010104351004791020150008291070026000";
            string paymentDate = "2022-12-30";

            CalculateFeeRequest request = new CalculateFeeRequest(barcode, paymentDate);

            Assert.AreEqual(barcode, request.Barcode);
            Assert.AreEqual(new DateTime(2022, 12, 30), request.PaymentDate);
        }

        [TestMethod]
        public void CalculateFeeRequest_InvalidDateFormat_ThrowsException()
        {
            string barcode = "34191790010104351004791020150008291070026000";
            string paymentDate = "invalid date";

            Assert.ThrowsException<BadHttpRequestException>(() => new CalculateFeeRequest(barcode, paymentDate));
        }
    }
}