using Microsoft.EntityFrameworkCore; 
using System; 
using System.ComponentModel.DataAnnotations; 
using System.ComponentModel.DataAnnotations.Schema; 

namespace dotnet_2.Infrastructure.Data.Models

{
    [Table("AuthTokenn")]
    public class AuthTokenn
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int id { get; set; }

        [Required]
        [Column("user_id", TypeName = "varchar(8)")]
        public int user_id { get; set; }

        [Required]
        [Column("token", TypeName = "varchar(500)")]
        public string? token { get; set; }

        [Required]
        [Column("role", TypeName = "varchar(200)")]
        public string? role { get; set; }

        [Required]
        [Column("expired_at", TypeName = "varchar(200)")]
        public int expired_at { get; set; }
    }
}

