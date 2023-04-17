using Builders.Bills.Payments;
using Moq;
using Moq.Protected;
using System.Net;

namespace Builders.Tests.Bills.Payments
{
    [TestClass]
    public class APIBaseTests
    {
        public class APIBaseMock : APIBase
        {
            public APIBaseMock(HttpClient httpClient) : base(httpClient)
            {
            }
        }

        public class JsonResponseMock
        {
            public string? Foo { get; set; }
        }

        private APIBase? APIBase;
        private string? urlMock;
        private string? requestBodyMock;
        private HttpResponseMessage responseMock;
        private Mock<HttpMessageHandler> mockHttpMessageHandler;
        private Mock<HttpClient> clientMock;

        [TestInitialize]
        public void TestInitialize()
        {
            mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            clientMock = new Mock<HttpClient>(mockHttpMessageHandler.Object);
            APIBase = new APIBaseMock(clientMock.Object);
            urlMock = "https://example.com/api/test";
            requestBodyMock = "{ \"test\": 123 }";
            responseMock = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("{ \"foo\": \"bar\" }")
            };
        }

        [TestMethod]
        public async Task SendPost_Should_Call_HttpClient_SendAsync()
        {
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(responseMock);

            var result = await APIBase.SendPost<JsonResponseMock>(urlMock, requestBodyMock);

            mockHttpMessageHandler.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(r =>
                    r.Method == HttpMethod.Post &&
                    r.RequestUri.ToString() == urlMock &&
                    r.Content.ReadAsStringAsync().Result == requestBodyMock),
                ItExpr.IsAny<CancellationToken>());
        }

        [TestMethod]
        public async Task SendPost_Should_Add_AuthorizationHeader_When_AuthIsProvided()
        {
            var authInfo = new AuthInfo("token", DateTime.Now.AddDays(1));

            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(responseMock);

            var dataResponse = await APIBase.SendPost<JsonResponseMock>(urlMock, requestBodyMock, authInfo);

            mockHttpMessageHandler.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(r =>
                    r.Headers.Authorization.ToString() == "token"),
                ItExpr.IsAny<CancellationToken>());
        }

        [TestMethod]
        public async Task SendPost_Should_Not_Add_AuthorizationHeader_When_AuthIsNotProvided()
        {
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(responseMock);

            var result = await APIBase.SendPost<JsonResponseMock>(urlMock, requestBodyMock);

            mockHttpMessageHandler.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(r =>
                    r.Headers.Authorization == null),
                ItExpr.IsAny<CancellationToken>());
        }

        [TestMethod]
        public async Task SendPost_Should_Return_an_APIResponse()
        {
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(responseMock);

            var result = await APIBase.SendPost<JsonResponseMock>(urlMock, requestBodyMock);

            Assert.IsInstanceOfType(result, typeof(APIResponse<JsonResponseMock>));
        }
    }
}