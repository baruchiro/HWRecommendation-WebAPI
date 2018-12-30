using System;

namespace HWWebApi.Models
{
    public class Memory
    {
        public long Id { get; set; }
        public int Capacity { get; set; }
        public RamType Type { get; set; } 
        public long Ghz { get; set; }

        public override bool Equals(object obj)
        {
            return obj is Memory memory &&
                   Capacity == memory.Capacity &&
                   Type == memory.Type &&
                   Ghz == memory.Ghz;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Capacity, Type, Ghz);
        }
    }
}