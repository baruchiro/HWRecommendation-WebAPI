using System;
using System.Linq;
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

        public override bool Equals(object obj)
        {
            return obj is Computer computer &&
                   Processor.Equals(computer.Processor) &&
                   Memories.All(computer.Memories.Contains) &&
                   Disks.All(computer.Disks.Contains) &&
                   MotherBoard.Equals(computer.MotherBoard) &&
                   GPUs.All(computer.GPUs.Contains);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Processor, Memories, Disks, MotherBoard, GPUs);
        }
    }
}