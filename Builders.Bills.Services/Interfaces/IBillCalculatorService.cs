using Builders.Bills.Shared;

namespace Builders.Bills.Services.Interfaces
{
    public interface IBillCalculatorService
    {
        Task<IBillCalculated?> GetCalculatedBill(string barcode, DateTime paymentDate);
    }
}