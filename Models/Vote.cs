using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MsalDemo.Backend.Models
{
    public class Vote
    {
        [Required]
        [MinLength(1)]
        public string Candidate { get; set; }
        
        [NotMapped]
        public string TenantId { get; set; }
        
        [NotMapped]
        public string ObjectId { get; set; }
        
        [NotMapped]
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.Now;
    }
}