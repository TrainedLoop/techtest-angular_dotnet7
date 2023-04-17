using Builders.Bills.Payments;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Json;

namespace Builders.Tests.Bills.Payments
{
    [TestClass]
    public class APIResponseTests
    {
        public class JsonResponseMock
        {
            public string? Name { get; set; }
            public int? Age { get; set; }
        }

        [TestMethod]
        public void APIResponse_SuccessfulResponse_ReturnsSuccessTrue()
        {
            HttpResponseMessage response = new(HttpStatusCode.OK)
            {
                Content = JsonContent.Create(new { Name = "John", Age = 30 })
            };

            APIResponse<JsonResponseMock> apiResponse = new(response);

            Assert.IsTrue(apiResponse.Success);
        }

        [TestMethod]
        public void APIResponse_SuccessfulResponse_ReturnsResult()
        {
            HttpResponseMessage response = new(HttpStatusCode.OK)
            {
                Content = JsonContent.Create(new { Name = "John", Age = 30 })
            };

            APIResponse<JsonResponseMock> apiResponse = new(response);

            Assert.IsNotNull(apiResponse.Result);
            Assert.AreEqual("John", apiResponse.Result.Name);
            Assert.AreEqual(30, apiResponse.Result.Age);
        }

        [TestMethod]
        public void APIResponse_FailedResponse_ReturnsSuccessFalse()
        {
            HttpResponseMessage response = new(HttpStatusCode.BadRequest)
            {
                Content = new StringContent("Bad Request")
            };

            APIResponse<JsonResponseMock> apiResponse = new(response);

            Assert.IsFalse(apiResponse.Success);
        }

        [TestMethod]
        public void APIResponse_FailedResponse_ReturnsReason()
        {
            HttpResponseMessage response = new(HttpStatusCode.BadRequest)
            {
                Content = new StringContent("Bad Request")
            };

            APIResponse<JsonResponseMock> apiResponse = new(response);

            Assert.IsNotNull(apiResponse.Reason);
            Assert.AreEqual("Bad Request", apiResponse.Reason);
        }

        [TestMethod]
        public void APIResponse_JsonDeserializationException_ThrowsJsonException()
        {
            HttpResponseMessage response = new(HttpStatusCode.OK)
            {
                Content = new StringContent("Invalid JSON")
            };

            Assert.ThrowsException<JsonException>(() => new APIResponse<JsonResponseMock>(response));
        }
    }
}