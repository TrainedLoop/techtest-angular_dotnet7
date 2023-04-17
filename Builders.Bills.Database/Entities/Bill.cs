using Builders.Bills.Shared;
using System.ComponentModel.DataAnnotations.Schema;

namespace Builders.Bills.Database.Models
{
    public class Bill : IBillCalculated
    {
        protected Bill()
        {
        }

        public Bill(IBillCalculated billCalculated, decimal fineRate, decimal interestRate)
        {
            this.Amount = billCalculated.Amount;
            this.OriginalAmount = billCalculated.OriginalAmount;
            this.DueDate = billCalculated.DueDate;
            this.PaymentDate = billCalculated.PaymentDate;
            this.FineAmountCalculated = billCalculated.FineAmountCalculated;
            this.InterestAmountCalculated = billCalculated.InterestAmountCalculated;

            this.FineRate = fineRate;
            this.InterestRate = interestRate;

            RequestDate = DateTime.Now;
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public decimal OriginalAmount { get; set; }
        public decimal Amount { get; set; }
        public string DueDate { get; set; }
        public string PaymentDate { get; set; }
        public decimal InterestAmountCalculated { get; set; }
        public decimal FineAmountCalculated { get; set; }
        public decimal FineRate { get; set; }
        public decimal InterestRate { get; set; }
        public DateTime RequestDate { get; set; }
    }
}