using Builders.Bills.Payments;
using Builders.Bills.Shared;
using Newtonsoft.Json;

namespace Builders.Bills.Services.BillCalculator
{
    public abstract class BillCalculated : IBillCalculated
    {
        public BillCalculated(Bill bill, DateTime paymentDate)
        {
            OriginalAmount = bill.Amount;
            DueDate = bill.DueDate.ToString("yyyy-MM-dd");// format not specified
            PaymentDate = paymentDate.ToString("yyyy-MM-dd");// format not specified
        }

        [JsonProperty(PropertyName = "original_amount")]
        public decimal OriginalAmount { get; }

        [JsonProperty(PropertyName = "amount")]
        public decimal Amount => OriginalAmount + InterestAmountCalculated + FineAmountCalculated;

        [JsonProperty(PropertyName = "due_date")]
        public string DueDate { get; }

        [JsonProperty(PropertyName = "payment_date")]
        public string PaymentDate { get; }

        [JsonProperty(PropertyName = "interest_amount_calculated")]
        public decimal InterestAmountCalculated { get; internal set; }

        [JsonProperty(PropertyName = "fine_amount_calculated")]
        public decimal FineAmountCalculated { get; internal set; }
    }
}