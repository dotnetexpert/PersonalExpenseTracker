using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class PayeeListUserMapping
    {
        [Key, Column(Order = 0)]
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        [Key, Column(Order = 1)]
        public int PayeeId { get; set; }
        [ForeignKey("PayeeId")]
        public PayeeList PayeeList { get; set; }
    }
}

