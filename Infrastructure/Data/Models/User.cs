using Microsoft.EntityFrameworkCore; 
using System; 
using System.ComponentModel.DataAnnotations; 
using System.ComponentModel.DataAnnotations.Schema; 

    namespace dotnet_2.Infrastructure.Data.Models{
    
    [Table("User")]
    public class User
    {   
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int id { get; set; }

        [Required]
        [Column("nik", TypeName = "varchar(8)")]
        public string? nik { get; set; }
        
        [Required]
        [Column("password", TypeName = "varchar(500)")]
        public string? password { get; set; }

        [Required]
        [Column("name", TypeName = "varchar(200)")]
        public string? name { get; set; }

        [Required]
        [Column("role", TypeName = "varchar(200)")]
        public string? role { get; set; }

        [Required]
        [Column("grade", TypeName = "varchar(10)")]
        public string? grade { get; set; }

        [Required]
        [Column("employment_status", TypeName = "varchar(30)")]
        public string? employment_status { get; set; }
        
        [Required]
        [Column("phone", TypeName = "varchar(20)")]
        public string? phone { get; set; }
        
        [Required]
        [Column("email", TypeName = "varchar(50)")]
        public string? email { get; set; }
        
        [Required]
        [Column("ktp", TypeName = "varchar(20)")]
        public string? ktp { get; set; }
        
        [Required]
        [Column("npwp", TypeName = "varchar(20)")]
        public string? npwp { get; set; }
        
        [Required]
        [Column("join_date", TypeName = "varchar(50)")]
        public string? join_date { get; set; }
        public Organization? organization { get; set; }
        public List<Overtime>? users { get; set; }
    }
    }