using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.CommonClass
{
    public class DomainObjects
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public bool IsDefault { get; set; }
        public bool isDeleted { get; set; }
        [NotMapped]
        public string ApplicationUserId { get; set; }
    }
}
