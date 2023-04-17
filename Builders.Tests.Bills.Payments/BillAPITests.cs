using Builders.Bills.Payments;
using Moq;

namespace Builders.Tests.Bills.Payments
{
    [TestClass]
    public class BillAPITests
    {
        private Mock<BillAPI> billAPiMock;

        [TestInitialize]
        public void TestInitialize()
        {
            billAPiMock = new Mock<BillAPI>(Mock.Of<HttpClient>())
            {
                CallBase = true
            };
        }

        [TestMethod]
        public async Task Auth_Should_CallSendPost()
        {
            var expectedJsonBody = @"{""client_id"":""client_id"",""client_secret"":""client_secret""}";
            var expectedUrl = "https://vagas.builders/api/builders/auth/tokens";

            billAPiMock.Setup(i => i.SendPost<AuthInfo>(It.IsAny<string>(), It.IsAny<string>(), null));

            var api = billAPiMock.Object;

            var response = await api.Auth("client_id", "client_secret");

            billAPiMock.Verify(i => i.SendPost<AuthInfo>
                (It.Is<string>(i => i == expectedUrl),
                 It.Is<string>(i => i == expectedJsonBody), null), Times.Once);
        }

        [TestMethod]
        public async Task GetInfo_Should_CallSendPost()
        {
            var expectedJsonBody = @"{""code"":""token""}";
            var authInfo = new AuthInfo("token", DateTime.Now.AddDays(1));
            var expectedUrl = "https://vagas.builders/api/builders/bill-payments/codes";

            billAPiMock.Setup(i => i.SendPost<Bill>(It.IsAny<string>(), It.IsAny<string>(), null));

            var api = billAPiMock.Object;

            var response = await api.GetInfo("token", authInfo);

            billAPiMock.Verify(i => i.SendPost<Bill>
                (It.Is<string>(i => i == expectedUrl),
                 It.Is<string>(i => i == expectedJsonBody),
                 It.Is<AuthInfo>(i => i.Token == authInfo.Token &&
                                  i.ExpiresIn == authInfo.ExpiresIn)), Times.Once);
        }
    }
}