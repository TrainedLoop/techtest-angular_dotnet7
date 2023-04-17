using Builders.Bills.Payments;
using Builders.Bills.Shared.Enums;

namespace Builders.Tests.Bills.Payments
{
    [TestClass]
    public class BillTests
    {
        [TestMethod]
        public void Valid_Constructor()
        {
            string code = "123456789";
            string dueDate = "2023-12-30";
            decimal amount = 100.0m;
            string recipientName = "John Smith";
            string recipientDocument = "123456789";
            string type = "NPC";

            var billInfo = new Bill(code, dueDate, amount, recipientName, recipientDocument, type);

            Assert.AreEqual(code, billInfo.Code);
            Assert.AreEqual(amount, billInfo.Amount);
            Assert.AreEqual(recipientName, billInfo.RecipientName);
            Assert.AreEqual(recipientDocument, billInfo.RecipientDocument);
            Assert.AreEqual(new DateTime(2023, 12, 30), billInfo.DueDate);
            Assert.AreEqual(BillType.NPC, billInfo.Type);
        }

        [TestMethod]
        public void Invalid_Constructor_InvalidType_ThrowsException()
        {
            string code = "123456789";
            string dueDate = "2023-04-30";
            decimal amount = 100.12m;
            string recipientName = "John Smith";
            string recipientDocument = "123456789";
            string type = "InvalidType";

            Assert.ThrowsException<NotImplementedException>(() => new Bill(code, dueDate, amount, recipientName, recipientDocument, type));
        }

        [TestMethod]
        public void Invalid_Constructor_InvalidDate_ThrowsException()
        {
            string code = "123456789";
            string dueDate = "2023-04-31"; // Invalid date
            decimal amount = 100.12m;
            string recipientName = "John Smith";
            string recipientDocument = "123456789";
            string type = "NPC";

            Assert.ThrowsException<ArgumentException>(() => new Bill(code, dueDate, amount, recipientName, recipientDocument, type));
        }

        [TestMethod]
        public void JsonSerialization()
        {
            var jsonString = @"
                {
                ""code"" : ""123456789"",
                ""amount"" : 100.12,
                ""due_date"" : ""2093-12-30"",
                ""recipient_name"" : ""John Smith"",
                ""recipient_document"" : ""123456789"",
                ""type"" : ""NPC""
                }";

            var billInfo = Newtonsoft.Json.JsonConvert.DeserializeObject<Bill>(jsonString);

            Assert.AreEqual("123456789", billInfo.Code);
            Assert.AreEqual(100.12m, billInfo.Amount);
            Assert.AreEqual("John Smith", billInfo.RecipientName);
            Assert.AreEqual("123456789", billInfo.RecipientDocument);
            Assert.AreEqual(new DateTime(2093, 12, 30), billInfo.DueDate);
            Assert.AreEqual(BillType.NPC, billInfo.Type);
        }
    }
}