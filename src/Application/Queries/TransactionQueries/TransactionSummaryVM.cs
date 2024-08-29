using System;
using System.Collections.Generic;

namespace Application.Queries.TransactionQueries
{
    public class TransactionSummaryVM
    {
        public double TotalIncome { get; set; }
        public double TotalExpense { get; set; }
        public double OutstandingBalance { get; set; }
        public IEnumerable<TransactionDetailVM> Transactions { get; set; }
    }

    public class TransactionDetailVM
    {
        public int TransactionId { get; set; }
        public double ExpenseAmount { get; set; }
        public DateTime TransactionDate { get; set; }
        public string Description { get; set; }
        public string TransactionType { get; set; } // "Credit" or "Debit"
        public ExpenseCategoryVM ExpenseCategory { get; set; }
        public PayeeVM Payee { get; set; }
        public PaymentMethodVM PaymentMethod { get; set; }
    }

    public class ExpenseCategoryVM
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
    }

    public class PayeeVM
    {
        public int PayeeId { get; set; }
        public string Name { get; set; }
    }

    public class PaymentMethodVM
    {
        public int PaymentMethodId { get; set; }
        public string Name { get; set; }
    }
}
