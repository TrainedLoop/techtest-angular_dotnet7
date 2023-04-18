using Builders.Bills.Database;
using Builders.Bills.Payments;
using Builders.Bills.Services.BillCalculator;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace Builders.Tests.Bills.Services
{
    [TestClass]
    public class BillCalculatorServiceTests
    {
        private readonly decimal fineRate = 2m;
        private readonly decimal interestRate = 1m;
        private readonly string clientId = "abc123";
        private readonly string clientSecret = "def456";
        private IConfigurationRoot configuration;
        private Mock<BillAPI> mockAPI;
        private Mock<IMemoryCache> mockCache;
        private Mock<BillsDbContext> dbContextMock;
        private Mock<DbSet<Builders.Bills.Database.Models.Bill>> dbSetMock;
        private APIResponse<AuthInfo> authResponseSuccess;

        [TestInitialize]
        public void TestInitialize()
        {
            var config = new BillCalculatorServiceConfig()
            {
                ClientId = clientId,
                ClientSecret = clientSecret,
                FineRate = fineRate,
                InterestRate = interestRate,
            };

            var appSettings = new
            {
                BillCalculator = config
            };
            var builder = new ConfigurationBuilder();
            builder.AddJsonStream(new MemoryStream(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(appSettings))));
            configuration = builder.Build();

            mockAPI = new Mock<BillAPI>(Mock.Of<HttpClient>());
            dbContextMock = new Mock<BillsDbContext>();
            dbSetMock = new Mock<DbSet<Builders.Bills.Database.Models.Bill>>();
            dbContextMock.Setup(m => m.Bills).Returns(dbSetMock.Object);

            mockCache = new Mock<IMemoryCache>();
            mockCache.Setup(i => i.CreateEntry(It.IsAny<object>())).Returns(Mock.Of<ICacheEntry>);

            var jsonAuth = @"{""token"" : ""123456789"", ""expires_in"" : ""2093-12-30""}";
            authResponseSuccess = new APIResponse<AuthInfo>(new(HttpStatusCode.OK)
            {
                Content = new StringContent(jsonAuth),
            });
        }

        [TestMethod]
        public async Task Test_GetCalculatedBill_Success_NPC()
        {
            //https://blog.juno.com.br/como-calcular-os-juros-e-multas-boleto-bancario/

            var paymentDate = DateTime.Now;
            var dueDate = DateTime.Now.AddDays(-10);

            var jsonBill = @$"
                            {{
                                ""code"": ""123456789"",
                                ""amount"": 500.0,
                                ""due_date"": ""{dueDate:yyyy-MM-dd}"",
                                ""recipient_name"": ""John Smith"",
                                ""recipient_document"": ""123456789"",
                                ""type"": ""NPC""
                            }}";

            var billResponseSuccess = new APIResponse<Bill>(
               new(HttpStatusCode.OK)
               {
                   Content = new StringContent(jsonBill),
               });

            var servicee = new BillCalculatorService(configuration, dbContextMock.Object, Mock.Of<HttpClient>(), mockCache.Object);
            mockAPI.Setup(i => i.GetInfo(It.IsAny<string>(), It.IsAny<AuthInfo>())).ReturnsAsync(billResponseSuccess);
            mockAPI.Setup(i => i.Auth(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(authResponseSuccess);

            servicee.API = mockAPI.Object;
            var result = await servicee.GetCalculatedBill("123456789", paymentDate);
            Assert.AreEqual(1.65m, result.InterestAmountCalculated);
            Assert.AreEqual(10m, result.FineAmountCalculated);
            Assert.AreEqual(dueDate.ToString("yyyy-MM-dd"), result.DueDate);
            Assert.AreEqual(paymentDate.ToString("yyyy-MM-dd"), result.PaymentDate);
            Assert.AreEqual(500m, result.OriginalAmount);
            Assert.AreEqual(511.65m, result.Amount);
            Assert.AreEqual("NPC", result.Type.ToString());
        }

        [TestMethod]
        public async Task Test_GetCalculatedBill_Success_NORNAL()
        {
            var paymentDate = DateTime.Now;
            var dueDate = DateTime.Now.AddDays(-10);

            var jsonBill = @$"
                            {{
                                ""code"": ""123456789"",
                                ""amount"": 500.0,
                                ""due_date"": ""{dueDate:yyyy-MM-dd}"",
                                ""recipient_name"": ""John Smith"",
                                ""recipient_document"": ""123456789"",
                                ""type"": ""NORMAL""
                            }}";

            var billResponseSuccess = new APIResponse<Bill>(
               new(HttpStatusCode.OK)
               {
                   Content = new StringContent(jsonBill),
               });

            var servicee = new BillCalculatorService(configuration, dbContextMock.Object, Mock.Of<HttpClient>(), mockCache.Object);
            mockAPI.Setup(i => i.GetInfo(It.IsAny<string>(), It.IsAny<AuthInfo>())).ReturnsAsync(billResponseSuccess);
            mockAPI.Setup(i => i.Auth(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(authResponseSuccess);

            servicee.API = mockAPI.Object;
            var result = await servicee.GetCalculatedBill("123456789", paymentDate);
            Assert.AreEqual(0m, result.InterestAmountCalculated);
            Assert.AreEqual(0m, result.FineAmountCalculated);
            Assert.AreEqual(dueDate.ToString("yyyy-MM-dd"), result.DueDate);
            Assert.AreEqual(paymentDate.ToString("yyyy-MM-dd"), result.PaymentDate);
            Assert.AreEqual(500m, result.OriginalAmount);
            Assert.AreEqual(500M, result.Amount);
            Assert.AreEqual("NORMAL", result.Type.ToString());
        }

        [TestMethod]
        public async Task Test_GetCalculatedBill_Success_AddToDB()
        {
            //https://blog.juno.com.br/como-calcular-os-juros-e-multas-boleto-bancario/

            var paymentDate = DateTime.Now;
            var dueDate = DateTime.Now.AddDays(-10);

            var jsonBill = @$"
                            {{
                                ""code"": ""123456789"",
                                ""amount"": 500.0,
                                ""due_date"": ""{dueDate:yyyy-MM-dd}"",
                                ""recipient_name"": ""John Smith"",
                                ""recipient_document"": ""123456789"",
                                ""type"": ""NPC""
                            }}";

            var billResponseSuccess = new APIResponse<Bill>(
               new(HttpStatusCode.OK)
               {
                   Content = new StringContent(jsonBill),
               });

            var servicee = new BillCalculatorService(configuration, dbContextMock.Object, Mock.Of<HttpClient>(), mockCache.Object);
            mockAPI.Setup(i => i.GetInfo(It.IsAny<string>(), It.IsAny<AuthInfo>())).ReturnsAsync(billResponseSuccess);
            mockAPI.Setup(i => i.Auth(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(authResponseSuccess);

            servicee.API = mockAPI.Object;
            var result = await servicee.GetCalculatedBill("123456789", paymentDate);

            dbSetMock.Verify(i => i.Add(It.IsAny<Builders.Bills.Database.Models.Bill>()), Times.Once);
            dbContextMock.Verify(i => i.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [TestMethod]
        public async Task Test_GetCalculatedBill_Return_Null_If_NotFound()
        {
            var paymentDate = DateTime.Now;
            var dueDate = DateTime.Now.AddDays(-10);

            var jsonBill = @$"
                            {{
                                ""code"": ""123456789"",
                                ""amount"": 500.0,
                                ""due_date"": ""{dueDate:yyyy-MM-dd}"",
                                ""recipient_name"": ""John Smith"",
                                ""recipient_document"": ""123456789"",
                                ""type"": ""NORMAL""
                            }}";

            var billResponseSuccess = new APIResponse<Bill>(
               new(HttpStatusCode.NotFound)
               {
                   Content = new StringContent("Not Found"),
               });

            var servicee = new BillCalculatorService(configuration, dbContextMock.Object, Mock.Of<HttpClient>(), mockCache.Object);

            mockAPI.Setup(i => i.Auth(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(authResponseSuccess);
            mockAPI.Setup(i => i.GetInfo(It.IsAny<string>(), It.IsAny<AuthInfo>())).ReturnsAsync(billResponseSuccess);

            servicee.API = mockAPI.Object;
            var result = await servicee.GetCalculatedBill("123456789", paymentDate);
            dbSetMock.Verify(i => i.Add(It.IsAny<Builders.Bills.Database.Models.Bill>()), Times.Never);
            dbContextMock.Verify(i => i.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task Test_GetCalculatedBill_Fail_When_InvalidResponse()
        {
            var billResponseFailure = new APIResponse<Bill>(
               new(HttpStatusCode.BadRequest)
               {
                   Content = new StringContent("BadRequest"),
               });

            var servicee = new BillCalculatorService(configuration, dbContextMock.Object, Mock.Of<HttpClient>(), mockCache.Object);
            mockAPI.Setup(i => i.Auth(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(authResponseSuccess);
            mockAPI.Setup(i => i.GetInfo(It.IsAny<string>(), It.IsAny<AuthInfo>())).ReturnsAsync(billResponseFailure);
            servicee.API = mockAPI.Object;

            dbSetMock.Verify(i => i.Add(It.IsAny<Builders.Bills.Database.Models.Bill>()), Times.Never);
            dbContextMock.Verify(i => i.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
            await Assert.ThrowsExceptionAsync<InvalidOperationException>(() => servicee.GetCalculatedBill("123456789", DateTime.Now));
        }

        [TestMethod]
        public async Task Test_GetCalculatedBill_Fail_When_InvalidAuth()
        {
            var authResponseFailure = new APIResponse<AuthInfo>(
                 new(HttpStatusCode.Unauthorized)
                 {
                     Content = new StringContent("Unauthorized"),
                 });

            var service = new BillCalculatorService(configuration, dbContextMock.Object, Mock.Of<HttpClient>(), mockCache.Object);
            mockAPI.Setup(i => i.Auth(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(authResponseFailure);
            service.API = mockAPI.Object;
            dbSetMock.Verify(i => i.Add(It.IsAny<Builders.Bills.Database.Models.Bill>()), Times.Never);
            dbContextMock.Verify(i => i.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
            await Assert.ThrowsExceptionAsync<InvalidOperationException>(() => service.GetCalculatedBill("123456789", DateTime.Now));
        }

        [TestMethod]
        public async Task Test_GetCalculated_Using_CacheAuth()
        {
            var paymentDate = DateTime.Now;
            var dueDate = DateTime.Now.AddDays(-10);
            object? cacheAuth = new AuthInfo("token", DateTime.Now.AddDays(1));
            mockCache.Setup(i => i.TryGetValue(It.IsAny<object>(), out cacheAuth)).Returns(true);

            var jsonBill = @$"
                            {{
                                ""code"": ""123456789"",
                                ""amount"": 500.0,
                                ""due_date"": ""{dueDate:yyyy-MM-dd}"",
                                ""recipient_name"": ""John Smith"",
                                ""recipient_document"": ""123456789"",
                                ""type"": ""NORMAL""
                            }}";

            var billResponseSuccess = new APIResponse<Bill>(
               new(HttpStatusCode.OK)
               {
                   Content = new StringContent(jsonBill),
               });

            var servicee = new BillCalculatorService(configuration, dbContextMock.Object, Mock.Of<HttpClient>(), mockCache.Object);
            mockAPI.Setup(i => i.GetInfo(It.IsAny<string>(), It.IsAny<AuthInfo>())).ReturnsAsync(billResponseSuccess);
            servicee.API = mockAPI.Object;
            var result = await servicee.GetCalculatedBill("123456789", paymentDate);
            Assert.AreEqual(0m, result.InterestAmountCalculated);
            Assert.AreEqual(0m, result.FineAmountCalculated);
            Assert.AreEqual(dueDate.ToString("yyyy-MM-dd"), result.DueDate);
            Assert.AreEqual(paymentDate.ToString("yyyy-MM-dd"), result.PaymentDate);
            Assert.AreEqual(500m, result.OriginalAmount);
            Assert.AreEqual(500M, result.Amount);
            Assert.AreEqual("NORMAL", result.Type.ToString());

        }
    }
}