namespace Application.Queries.TransactionQueries
{
    public class GetAllTransactionsByTransactionIdVM
    {
        public TransactionSummaryVM Today { get; set; }
        public TransactionSummaryVM Last7Days { get; set; }

        public TransactionSummaryVM ThisMonth { get; set; }
        public TransactionSummaryVM ThisYear { get; set; }
    }
}
