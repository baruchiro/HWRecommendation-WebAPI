using System;
using System.Collections.Generic;
using HWWebApi.Helpers;
using Models.ModelEqualityComparer;

namespace Models
{
    public class Computer : IModel<Computer>
    {
        public Computer() { }
        protected Computer(Computer computer)
        {
            Id = computer.Id;
            Processor = computer.Processor;
            Memories = computer.Memories;
            Disks = computer.Disks;
            MotherBoard = computer.MotherBoard;
            Gpus = Gpus;
        }

        public long Id { get; set; }
        public Processor Processor { get; set; } 
        public ICollection<Memory> Memories { get; set; } = new List<Memory>();
        public ICollection<Disk> Disks { get; set; } = new List<Disk>();
        public MotherBoard MotherBoard { get; set; } 
        public ICollection<Gpu> Gpus { get; set; } = new List<Gpu>();

        public bool EqualByMembers(Computer computer)
        {
            return ModelEqualityByMembers<Processor>.EqualByMembers(Processor, computer.Processor) &&
                   Memories.IsEquals(computer.Memories, new ModelEqualityByMembers<Memory>()) &&
                   Disks.IsEquals(computer.Disks, new ModelEqualityByMembers<Disk>()) &&
                   ModelEqualityByMembers<MotherBoard>.EqualByMembers(MotherBoard, computer.MotherBoard) &&
                   Gpus.IsEquals(computer.Gpus, new ModelEqualityByMembers<Gpu>());
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
                hashCode = (hashCode * 397) ^ (Gpus?.GetHashCode() ?? 0);
                return hashCode;
            }
        }
    }
}