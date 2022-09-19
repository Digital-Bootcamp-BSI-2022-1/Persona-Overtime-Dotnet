using Microsoft.EntityFrameworkCore; 
using System; 
using System.ComponentModel.DataAnnotations; 
using System.ComponentModel.DataAnnotations.Schema;

    namespace dotnet_2.Infrastructure.Data.Models{
    public class Overtime
    {   
        public int id { get; set; }
        public User? user { get; set; }
        public DateOnly start_date { get; set; }
        public DateOnly end_date { get; set; }
        public TimeOnly start_time { get; set; }
        public TimeOnly end_time { get; set; }
        public int duration { get; set; }
        public int status { get; set; }
        public string? status_text { get; set; }
        public int is_completed { get; set; }
        public string? remarks { get; set; }
        public string? attachment { get; set; }
        public DateOnly request_date { get; set; }
        public TimeOnly request_time { get; set; }
        public DateOnly approved_date { get; set; }
        public TimeOnly approved_time { get; set; }
        public DateOnly completed_date { get; set; }
        public TimeOnly completed_time { get; set; }
    }
}

