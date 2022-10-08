using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnet_2.Infrastructure.Dto
{
    public class OvertimeStatistic
    {
        public int need_approval {get; set;}
        public int approved {get; set;}
        public int rejected {get; set;}
        public int settlement_approval {get; set;}
        public int revise {get; set;}
        public int cancelled {get; set;}
        public int completed {get; set;}
    }
}