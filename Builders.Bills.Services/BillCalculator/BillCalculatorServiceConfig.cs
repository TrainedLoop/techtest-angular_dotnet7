namespace Builders.Bills.Services.BillCalculator
{
    public class BillCalculatorServiceConfig
    {
        public string? ClientId { get; set; }
        public string? ClientSecret { get; set; }
        public decimal? InterestRate { get; set; }
        public decimal? FineRate { get; set; }
    }
}