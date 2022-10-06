using Microsoft.EntityFrameworkCore; 
using System; 
using System.ComponentModel.DataAnnotations; 
using System.ComponentModel.DataAnnotations.Schema; 

    namespace dotnet_2.Infrastructure.Data.Models{
    
    [Table("Organization")]
    public class Organization
    {   
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int id { get; set; }

        [Required]
        [Column("organization_name", TypeName = "varchar(200)")]
        public string? organization_name { get; set; }
        public User? head { get; set; }
        public List<User>? member { get; set; }
    }
    }