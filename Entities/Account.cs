using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    public class Account
    {
        public int AccountId { get; set; }
        public string? Name { get; set; }
        public Guid AccountNumber { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal Balance { get; set; }
    }
}