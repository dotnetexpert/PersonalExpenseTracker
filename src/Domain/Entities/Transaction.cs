using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class Transaction
    {
        public int TransactionId { get; set; }

        public TransactionType TransactionTypeId { get; set; }

        public double ExpenseAmount { get; set; }

        public string ApplicationUserId { get; set; }

        public ApplicationUser ApplicationUser { get; set; }

        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public ExpenseCategory ExpenseCategory { get; set; }

        public int PaymentMethodId { get; set; }

        [ForeignKey("PaymentMethodId")]
        public PaymentMethod PaymentMethod { get; set; }

        public int PayeeId { get; set; }

        [ForeignKey("PayeeId")]
        public PayeeList PayeeList { get; set; }

        public string Description { get; set; }

        public DateTime TransactionDate { get; set; }
    }

    public enum TransactionType
    {
        Debit,
        Credit
    }
}
