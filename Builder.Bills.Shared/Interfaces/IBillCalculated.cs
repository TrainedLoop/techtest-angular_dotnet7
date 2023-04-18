using Builders.Bills.Shared.Enums;

namespace Builders.Bills.Shared
{
    public interface IBillCalculated
    {
        decimal OriginalAmount { get; }
        decimal Amount { get; }
        string DueDate { get; }
        string PaymentDate { get; }
        decimal InterestAmountCalculated { get; }
        decimal FineAmountCalculated { get; }
        BillType Type { get; }
    }
}