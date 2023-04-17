using Builders.Bills.API.Models;
using Builders.Bills.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Builders.Bills.API.Controllers
{
    /// <summary>
    /// API controller for calculating bill fees.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class BillController : ControllerBase
    {
        private readonly IBillCalculatorService billCalculator;

        /// <summary>
        /// Initializes a new instance of the <see cref="BillController"/> class.
        /// </summary>
        /// <param name="billCalculator">The bill calculator service.</param>
        ///
        public BillController(IBillCalculatorService billCalculator)
        {
            this.billCalculator = billCalculator;
        }

        /// <summary>
        /// Calculates the fees for a bill.
        /// </summary>
        /// <param name="body">The request body containing the barcode and payment date.</param>
        /// <response code="200">Returns the calculated bill fees.</response>
        /// <response code="400">If the request body is null or invalid.</response>
        /// <response code="404">If the bill with the given barcode is not found.</response>
        /// <returns>The calculated bill fees.</returns>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CalculateFeeRequest body)
        {
            var calculatedBill = await billCalculator.GetCalculatedBill(body.Barcode, body.PaymentDate);

            if (calculatedBill == null)
            {
                return NotFound();
            }

            return Ok(calculatedBill);
        }
    }
}