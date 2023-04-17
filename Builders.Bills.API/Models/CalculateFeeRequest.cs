using Newtonsoft.Json;
using System.Globalization;

namespace Builders.Bills.API.Models
{
    /// <summary>
    /// Represents the request body for calculating fees for a bill.
    /// </summary>
    public class CalculateFeeRequest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CalculateFeeRequest"/> class
        /// with the specified barcode and payment date.
        /// </summary>
        /// <param name="bar_code">The barcode number from the bill.</param>
        /// <param name="payment_date">The payment date that will be used to calculate the fees.</param>
        public CalculateFeeRequest(string bar_code, string payment_date)
        {
            Barcode = bar_code;
            if (DateTime.TryParseExact(payment_date, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime result))
            {
                PaymentDate = result;
            }
            else
                throw new BadHttpRequestException($"{payment_date} is a invalid date format for {nameof(payment_date)}");
        }

        /// <summary>
        /// the barcode number from the bill.
        /// </summary>
        /// <example>34191790010104351004791020150008291070026000</example>
        [JsonProperty(propertyName: "bar_code")]
        public string Barcode { get; }

        /// <summary>
        /// payment date that will be used to calculate fees.
        /// </summary>
        /// <example>2022-12-30</example>

        [JsonProperty(propertyName: "payment_date")]
        public DateTime PaymentDate { get; }
    }
}