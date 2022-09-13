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
        [Column("start_date", TypeName = "date")]
        public DateOnly start_date { get; set; }

        [Required]
        [Column("end_date", TypeName = "date")]
        public DateOnly end_date { get; set; }

        [Required]
        [Column("start_time", TypeName = "time")]
        public TimeOnly start_time { get; set; }

        [Required]
        [Column("end_time", TypeName = "time")]
        public TimeOnly end_time { get; set; }

        [Required]
        [Column("status", TypeName = "int")]
        public int status { get; set; }
        
        [Required]
        [Column("is_completed", TypeName = "int")]
        public int is_completed { get; set; }
        
        [Required]
        [Column("remarks", TypeName = "varchar(200)")]
        public string? remarks { get; set; }
        
        [Required]
        [Column("attachment", TypeName = "varchar(500)")]
        public string? attachment { get; set; }
        
        [Required]
        [Column("request_date", TypeName = "date")]
        public DateOnly request_date { get; set; }

        [Required]
        [Column("request_time", TypeName = "time")]
        public TimeOnly request_time { get; set; }

        [Required]
        [Column("approved_date", TypeName = "date")]
        public DateOnly approved_date { get; set; }

        [Required]
        [Column("approved_time", TypeName = "time")]
        public TimeOnly approved_time { get; set; }

        [Required]
        [Column("completed_date", TypeName = "date")]
        public DateOnly completed_date { get; set; }

        [Required]
        [Column("completed_time", TypeName = "time")]
        public TimeOnly completed_time { get; set; }
    }
    }