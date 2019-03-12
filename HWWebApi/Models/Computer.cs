using System.Collections.Generic;
using HWWebApi.Helpers;
using HWWebApi.Models.ModelEqualityComparer;

namespace HWWebApi.Models
{
    public class Computer : IModel<Computer>
    {
        public long Id { get; set; }
        public Processor Processor { get; set; } 
        public ICollection<Memory> Memories { get; set; }
        public ICollection<Disk> Disks { get; set; } 
        public MotherBoard MotherBoard { get; set; } 
        public ICollection<GPU> GPUs { get; set; }

        public bool EqualByMembers(Computer computer)
        {
            return Processor.EqualByMembers(computer.Processor) &&
                   Memories.IsEquals(computer.Memories, new ModelEqualityByMembers<Memory>()) &&
                   Disks.IsEquals(computer.Disks, new ModelEqualityByMembers<Disk>()) &&
                   MotherBoard.EqualByMembers(computer.MotherBoard) &&
                   GPUs.IsEquals(computer.GPUs, new ModelEqualityByMembers<GPU>());
        }

        public int GetHashCodeWithMembers()
        {
            unchecked
            {
                var hashCode = 397;
                hashCode = (hashCode * 397) ^ (Processor?.GetHashCode() ?? 0);
                hashCode = (hashCode * 397) ^ (Memories?.GetHashCode() ?? 0);
                hashCode = (hashCode * 397) ^ (Disks?.GetHashCode() ?? 0);
                hashCode = (hashCode * 397) ^ (MotherBoard?.GetHashCode() ?? 0);
                hashCode = (hashCode * 397) ^ (GPUs?.GetHashCode() ?? 0);
                return hashCode;
            }
        }
    }
}