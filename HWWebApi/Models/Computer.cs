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
            var he = obj as Computer;

            return
                he != null &&
                this.Id.Equals(he.Id) &&
                this.Memories.All(he.Memories.Contains) &&
                this.Disks.All(he.Disks.Contains) &&
                this.GPUs.All(he.GPUs.Contains) &&
                this.MotherBoard.Equals(he.MotherBoard) &&
                this.Processor.Equals(he.Processor);
        }
    }
}