﻿using Builders.Bills.Database.Models;
using Builders.Bills.Shared;

namespace Builders.Bills.Database.Tests
{
    [TestClass]
    public class BillTests
    {
        internal class BillCalculatedMock : IBillCalculated
        {
            public BillCalculatedMock(decimal originalAmount, decimal amount, string dueDate, string paymentDate, decimal fineAmountCalculated, decimal interestAmountCalculated)
            {
                OriginalAmount = originalAmount;
                Amount = amount;
                DueDate = dueDate;
                PaymentDate = paymentDate;
                FineAmountCalculated = fineAmountCalculated;
                InterestAmountCalculated = interestAmountCalculated;
            }

            public decimal OriginalAmount { get; private set; }
            public decimal Amount { get; private set; }
            public string DueDate { get; private set; }
            public string PaymentDate { get; private set; }
            public decimal FineAmountCalculated { get; private set; }
            public decimal InterestAmountCalculated { get; private set; }
        }

        [TestMethod]
        public void Constructor_ShouldInitializeProperties()
        {
            decimal originalAmount = 100.00M;
            decimal amount = 105.00M;
            string dueDate = "2022-05-15";
            string paymentDate = "2022-05-16";
            decimal fineAmountCalculated = 5.00M;
            decimal interestAmountCalculated = 0.00M;
            decimal fineRate = 0.05M;
            decimal interestRate = 0.00M;
            var billCalculated = new BillCalculatedMock(originalAmount, amount, dueDate, paymentDate, fineAmountCalculated, interestAmountCalculated);

            var bill = new Bill(billCalculated, fineRate, interestRate);

            Assert.AreEqual(originalAmount, bill.OriginalAmount);
            Assert.AreEqual(amount, bill.Amount);
            Assert.AreEqual(dueDate, bill.DueDate);
            Assert.AreEqual(paymentDate, bill.PaymentDate);
            Assert.AreEqual(fineAmountCalculated, bill.FineAmountCalculated);
            Assert.AreEqual(interestAmountCalculated, bill.InterestAmountCalculated);
            Assert.AreEqual(fineRate, bill.FineRate);
            Assert.AreEqual(interestRate, bill.InterestRate);
            Assert.IsTrue(bill.RequestDate > DateTime.MinValue);
        }
    }
}