using CashFlow.Domain.Entities;
using DocumentFormat.OpenXml.Spreadsheet;

namespace WebApi.Test.Resources
{
    public class ExpenseIdentityManager
    {
        private Expense _expense;

        public ExpenseIdentityManager(Expense expense)
        {
            _expense = expense;
        }

        public long GetExpenseId() => _expense.Id;

        public DateTime GetDate() => _expense.Date;
    }
}
