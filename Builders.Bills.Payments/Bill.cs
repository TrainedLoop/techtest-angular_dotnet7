using Builders.Bills.Shared.Enums;
using System.Globalization;

namespace Builders.Bills.Payments
{
    public class Bill
    {
        public Bill(string code,
                    string due_date,
                    decimal amount,
                    string recipient_name,
                    string recipient_document,
                    string type)
        {
            Code = code;
            Amount = amount;
            RecipientName = recipient_name;
            RecipientDocument = recipient_document;
            SetBillType(type);
            SetDueDate(due_date);
        }

        public string Code { get; }
        public decimal Amount { get; }
        public string RecipientName { get; }
        public string RecipientDocument { get; }
        public DateTime DueDate { get; private set; }
        public BillType Type { get; private set; }

        private void SetBillType(string type)
        {
            if (Enum.TryParse(type, true, out BillType billType))
                Type = billType;
            else
                throw new NotImplementedException($"{nameof(billType)} of type ${type} is not implemented");
        }

        private void SetDueDate(string dueDate)
        {
            //está errado na doc do test tem que ser 'yyyy-MM-dd'
            if (DateTime.TryParseExact(dueDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime result))
            {
                DueDate = result;
            }
            else
                throw new ArgumentException($"{dueDate} is a invalid date format for {nameof(DueDate)}");
        }
    }
}