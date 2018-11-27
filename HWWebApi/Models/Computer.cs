using System;
using System.Collections.Generic;

namespace HWWebApi.Models
{
    public class Computer
    {
        public long Id { get; set; }
        public Processor processor { get; set; }
        public ICollection<Memory> memories { get; set; }
        public ICollection<Disk> disks { get; set; }
        public MotherBoard motherBoard { get; set; }
        public ICollection<GPU> gpus { get; set; }
    }
}