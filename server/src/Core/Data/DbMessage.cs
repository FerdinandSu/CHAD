using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Chad.Data
{
    [Index(nameof(SenderId), nameof(ReceiverId))]
    public class DbMessage
    {
        public long Id { get; set; }

        [MaxLength(256)] [Required] public string Content { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime Time { get; set; }

        [Required] public DbUser Sender { get; set; }

        [Required] public DbUser Receiver { get; set; }

        [Required] public string SenderId { get; set; }

        [Required] public string ReceiverId { get; set; }
    }
}