using Builders.Bills.Payments;

namespace Builders.Bills.Services.BillCalculator
{
    internal sealed class BillCalculatedNormal : BillCalculated
    {
        internal BillCalculatedNormal(Bill bill, DateTime paymentDate) : base(bill, paymentDate)
        {
        }
    }
}