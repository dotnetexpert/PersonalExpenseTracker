using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class ExpenseUserMapping
    {
        [Key, Column(Order = 0)]
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        [Key, Column(Order = 1)]
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public ExpenseCategory ExpenseCategory { get; set; }
    }
}
