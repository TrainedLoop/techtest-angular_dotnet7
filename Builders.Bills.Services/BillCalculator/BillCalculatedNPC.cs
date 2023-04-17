using Builders.Bills.Payments;

namespace Builders.Bills.Services.BillCalculator
{
    internal sealed class BillCalculatedNPC : BillCalculated
    {
        private readonly DateTime dueDate;
        private readonly DateTime paymentDate;

        internal BillCalculatedNPC(Bill bill, DateTime paymentDate) : base(bill, paymentDate)
        {
            this.dueDate = bill.DueDate;
            this.paymentDate = paymentDate;
        }

        internal BillCalculatedNPC CalculateFees(decimal interestRate, decimal fineRate)
        {
            var interestRatePerDay = Math.Truncate((interestRate / 30) * 1000) / 1000;
            var overdueDays = (paymentDate.Date - dueDate.Date).Days;

            var interrestValue = OriginalAmount / 100;

            if (overdueDays > 0)
            {
                InterestAmountCalculated = interrestValue * interestRatePerDay * overdueDays;
                FineAmountCalculated = interrestValue * fineRate;
            }
            return this;
        }
    }
}