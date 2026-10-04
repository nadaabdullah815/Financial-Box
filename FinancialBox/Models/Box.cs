using System;
using System.ComponentModel.DataAnnotations;
namespace FinancialBox.Models
{
    public class Box
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Currency { get; set; } = string.Empty;
        public decimal Balance { get; set; }
          public decimal? TargetAmount { get; set; } 
           public bool IsDefault { get; set; }
           public bool IsDebtBox { get; set; } 
         public string UserId { get; set; }  = string.Empty;
         public ApplicationUser User { get; set; } = null!;         
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}