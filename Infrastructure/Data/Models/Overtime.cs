using Microsoft.EntityFrameworkCore; 
using System; 
using System.ComponentModel.DataAnnotations; 
using System.ComponentModel.DataAnnotations.Schema;

    namespace dotnet_2.Infrastructure.Data.Models{
    
    [Table("Overtime")]
    public class Overtime
    {   
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int id { get; set; }

        [Required]
        [Column("user")]
        public User? user { get; set; }
        
        [Required]
        [Column("start_date", TypeName = "varchar(20)")]
        public String? start_date { get; set; }

        [Required]
        [Column("end_date", TypeName = "varchar(20)")]
        public String? end_date { get; set; }

        [Required]
        [Column("start_time", TypeName = "varchar(20)")]
        public String? start_time { get; set; }

        [Required]
        [Column("end_time", TypeName = "varchar(20)")]
        public String? end_time { get; set; }

        [Required]
        [Column("status", TypeName = "int")]
        public int status { get; set; }

        [Column("status_text", TypeName = "varchar(50)")]
        public string? status_text { get; set; }
        
        [Required]
        [Column("is_completed", TypeName = "int")]
        public int is_completed { get; set; }
        
        [Column("remarks", TypeName = "varchar(200)")]
        public string? remarks { get; set; }
        
        [Column("attachment", TypeName = "varchar(500)")]
        public string? attachment { get; set; }
        
        [Required]
        [Column("request_date", TypeName = "varchar(20)")]
        public String? request_date { get; set; }

        [Required]
        [Column("request_time", TypeName = "varchar(20)")]
        public String? request_time { get; set; }

        [Required]
        [Column("approved_date", TypeName = "varchar(20)")]
        public String? approved_date { get; set; }

        [Required]
        [Column("approved_time", TypeName = "varchar(20)")]
        public String? approved_time { get; set; }

        [Required]
        [Column("completed_date", TypeName = "varchar(20)")]
        public String? completed_date { get; set; }

        [Required]
        [Column("completed_time", TypeName = "varchar(20)")]
        public String? completed_time { get; set; }
    }
}

