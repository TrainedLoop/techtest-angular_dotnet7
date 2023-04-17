using Builders.Bills.Payments;

namespace Builders.Tests.Bills.Payments
{
    [TestClass]
    public class AuthInfoTests
    {
        [TestMethod]
        public void Constructor_Valid()
        {
            string token = "abc123";
            DateTime expiresIn = DateTime.Now.AddHours(1);

            var response = new AuthInfo(token, expiresIn);

            Assert.AreEqual(token, response.Token);
            Assert.AreEqual(expiresIn, response.ExpiresIn);
        }

        [TestMethod]
        public void Invalid_Constructor_NullToken_ThrowsException()
        {
            string? token = null;
            DateTime expiresIn = DateTime.Now.AddHours(1);

            Assert.ThrowsException<ArgumentNullException>(() => new AuthInfo(token, expiresIn));
        }

        [TestMethod]
        public void Invalid_Constructor_ExpiredExpiresIn_ThrowsException()
        {
            string token = "abc123";
            DateTime expiresIn = DateTime.Now.AddHours(-1);

            Assert.ThrowsException<ArgumentException>(() => new AuthInfo(token, expiresIn));
        }

        [TestMethod]
        public void JsonSerialization()
        {
            var jsonString = @"
                {
                ""token"" : ""123456789"",
                ""expires_in"" : ""2093-12-30T14:29:10.976""
                }";

            var authInfo = Newtonsoft.Json.JsonConvert.DeserializeObject<AuthInfo>(jsonString);

            Assert.AreEqual("123456789", authInfo.Token);
            Assert.AreEqual(new DateTime(2093, 12, 30, 14, 29, 10, 976), authInfo.ExpiresIn);
        }
    }
}