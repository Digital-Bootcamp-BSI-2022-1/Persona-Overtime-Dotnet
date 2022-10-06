using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnet_2.Infrastructure.Data.Models
{
    public class WorkSchedule
    {
        public int id { get; set; }
        public string? work_shedule { get; set; }
        public TimeOnly start_time { get; set; }
        public TimeOnly end_time { get; set; }
        public TimeOnly start_break_time1 { get; set; }
        public TimeOnly end_break_time1 { get; set; }
        public TimeOnly start_break_time2 { get; set; }
        public TimeOnly end_break_time2 { get; set; }

    }
}