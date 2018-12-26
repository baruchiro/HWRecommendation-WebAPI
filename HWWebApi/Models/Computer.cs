using System;
using System.Collections.Generic;

namespace HWWebApi.Models
{
    public class Computer
    {
        public long Id { get; set; }
        public Processor Processor { get; set; }
        public ICollection<Memory> Memories { get; set; }
        public ICollection<Disk> Disks { get; set; }
        public MotherBoard MotherBoard { get; set; }
        public ICollection<GPU> GPUs { get; set; }
    }
}