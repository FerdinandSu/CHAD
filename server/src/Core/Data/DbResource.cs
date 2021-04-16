using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Chad.Data
{
    public class DbResource
    {
        public long Id { get; set; }

        [Required] [MaxLength(32)] public string Name { get; set; }

        [MaxLength(4194304)] public byte[] Content { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime UploadTime { get; set; }

        [Required] public DateTime Expired { get; set; }

        [Required] public DbUser Uploader { get; set; }

        [Required] public string UploaderId { get; set; }

        [Required] public string ContentType { get; set; }
    }
}